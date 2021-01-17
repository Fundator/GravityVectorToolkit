using GravityVectorToolKit.Common;
using GravityVectorToolKit.Common.Extensions;
using GravityVectorToolKit.CSV.Mapping;
using GravityVectorToolKit.DataAccess;
using GravityVectorToolKit.DataModel;
using log4net;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Valid;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GravityVectorToolkit.Tools.DatabaseImport
{

	public static class DatabaseImport
	{
		private static ILog Logger = LogManager.GetLogger(typeof(DatabaseImport));
		private static object syncRoot = new object();
		private static Action<string> UpdateStatus = null;

		public static List<ValidationError> Validate(DatabaseImportParameters parameters)
		{
			var result = new List<ValidationError>();

			if (!string.IsNullOrWhiteSpace(parameters.NormalRoutePath))
			{
				if (!File.Exists(parameters.NormalRoutePath))
				{
					result.Add(GenerateError($"The file {parameters.NormalRoutePath} does not exist"));
				}
			}

			if (!string.IsNullOrWhiteSpace(parameters.GravityVectorPath))
			{
				if (string.IsNullOrWhiteSpace(parameters.NormalRoutePath))
				{
					result.Add(GenerateError("If you specify a gravity vector path, you must also specify a normal route path"));
				}
				if (!File.Exists(parameters.GravityVectorPath))
				{
					result.Add(GenerateError($"The file {parameters.GravityVectorPath} does not exist"));
				}
			}

			if (!string.IsNullOrWhiteSpace(parameters.DeviationMapPath))
			{
				if (!File.Exists(parameters.DeviationMapPath))
				{
					result.Add(GenerateError($"The file {parameters.DeviationMapPath} does not exist"));
				}
			}

			if (string.IsNullOrWhiteSpace(parameters.ConnectionString))
			{
				result.Add(GenerateError("You must specify a connection string"));
			}
			else
			{
				try
				{
					FluentConfiguration.TestConfiguration(parameters.ConnectionString);
				}
				catch (ArgumentException e)
				{
					result.Add(GenerateError("Invalid connection string: " + e.Message));
				}
				catch (Exception e)
				{
					result.Add(GenerateError(e.Message));
				}
			}

			return result;
		}

		private static ValidationError GenerateError(string errorMessage)
		{
			return new ValidationError
			{
				ErrorMessage = errorMessage
			};
		}

		private static void Log(string message)
		{
			Logger.Info(message);
			if (UpdateStatus != null)
			{
				UpdateStatus(message);
			}
		}

		public static void Import(DatabaseImportParameters parameters, Action<string> updateStatus)
		{
			UpdateStatus = updateStatus;

			// Configure database 
			FluentConfiguration.Configure(parameters.ConnectionString, parameters.DropAndCreate);

			var normalRoutes = new List<NormalRoute>();

			// Import normal routes
			if (!string.IsNullOrWhiteSpace(parameters.NormalRoutePath))
			{
				normalRoutes = ImportNormalRoutes(parameters.NormalRoutePath);
			}

			// Import gravity vectors
			if (!string.IsNullOrWhiteSpace(parameters.GravityVectorPath))
			{
				ImportGravityVectors(syncRoot, parameters.GravityVectorPath, normalRoutes);
			}

			// Import deviation map
			if (!string.IsNullOrWhiteSpace(parameters.DeviationMapPath))
			{
				ImportDeviationCells(parameters.DeviationMapPath);
			}

		}

		public static List<NormalRoute> ImportNormalRoutes(string path)
		{
			List<NormalRoute> allNormalRoutes = new List<NormalRoute>();
			var filename = Path.GetFileName(path);
			Log($"Loading normal routes from {filename}..");
			var syncRoot = new object();
			const int batchSize = 150;
			int rowCount = 0;
			using (var input = new StreamReader(File.OpenRead(path)))
			{
				var header = input.ReadLine(); // Keep the header
				Parallel.ForEach(
					input.ReadLines().TakeChunks(batchSize),
					new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },
					lines =>
					{
						var linesWithHeader = new List<string> { header }
										.Concat(lines);

						var normalRoutes = Util.ReadFromList<NormalRoute, NormalRouteCsvClassMap>(linesWithHeader);

						ISession nrSession = Util.GetSession();
						ITransaction transaction = Util.BeginTransaction(nrSession);
						foreach (var normalRoute in normalRoutes)
						{
							Interlocked.Increment(ref rowCount);
							nrSession.SaveOrUpdate(normalRoute); // Use Save() because the primary is assigned
							if (rowCount % 100 == 0)
							{
								Log($"Processed {rowCount} normal routes");
							}
						}
						transaction.Commit();
						nrSession.Flush();
						nrSession.Close();
						lock (syncRoot)
						{
							allNormalRoutes.AddRange(normalRoutes);
						}
					});
			}
			Log($"Done processing {rowCount} normal routes");

			return allNormalRoutes;
		}

		public static void ImportGravityVectors(object syncRoot, string file, List<NormalRoute> normalRoutes)
		{
			Log($"Loading gravity vectors..");

			var normalRouteMap = normalRoutes.ToDictionary(x => x.NormalRouteId, x => x);

			var gvMap = new Dictionary<string, List<GravityVector>>();

			foreach (var normalRoute in normalRoutes)
			{
				gvMap.Add(normalRoute.NormalRouteId, new List<GravityVector>());
			}

			try
			{
				using (var input = new StreamReader(File.OpenRead(file)))
				{
					var header = input.ReadLine();
					var filename = Path.GetFileName(file);
					var rowCount = 0;
					const int batchSize = 10000;
					Log($"Loading gravity vectors from {Path.GetFileName(file)}..");
					Parallel.ForEach(input.ReadLines().TakeChunks(batchSize),
										new ParallelOptions()
										{
											MaxDegreeOfParallelism = Environment.ProcessorCount
										},
										lines =>
					{
						var linesWithHeader = new List<string> { header }.Concat(lines);
						try
						{
							var session = Util.GetSession();
							var transaction = Util.BeginTransaction(session);
							var records = Util.ReadFromList<GravityVector, GravityVectorCsvClassMap>(linesWithHeader);
							foreach (var gravityVector in records)
							{
								Interlocked.Increment(ref rowCount);
								gvMap[gravityVector.NormalRouteId].Add(gravityVector);
								var normalRoute = normalRouteMap[gravityVector.NormalRouteId];
								gravityVector.NormalRoute = normalRoute;
								if (normalRoute.GravityVectors == null)
								{
									normalRoute.GravityVectors = new List<GravityVector>();
								}
								normalRoute.GravityVectors.Add(gravityVector);
								session.SaveOrUpdate(gravityVector);
							}

							Log($"Processed {rowCount} rows..");
							transaction.Commit();
							session.Flush();
							session.Close();
							session.Dispose();
						}
						catch (Exception e)
						{
							Log(e.Message);
						}
					});
					Log($"Done processing {rowCount} gravity vectors");
					Log($"Generating convex hulls..");
					var i = 0;
					var session = Util.GetSession();
					var transaction = Util.BeginTransaction(session);
					foreach (var normalRoute in normalRoutes)
					{
						i++;
						// Sort the gravity vectors since they are probably out of order due to parallell processing
						gvMap[normalRoute.NormalRouteId] = gvMap[normalRoute.NormalRouteId].OrderBy(gv => gv.SerialId).ToList();
						GenerateConvexHulls(gvMap[normalRoute.NormalRouteId], normalRoute);
						session.SaveOrUpdate(normalRoute);
						if (i % 100 == 0)
						{
							Log($"Created convex hulls for {i} normal routes..");
						}
					}
					Log("Committing convex hulls to database..");
					transaction.Commit();
					session.Flush();
					session.Close();
					session.Dispose();
					Log($"Done generating convex hulls");
				}
			}
			catch (Exception e)
			{
				Log(e.Message);
			}
		}

		public static void ImportDeviationCells(string path)
		{
			Log($"Loading deviation cells..");

			var batchSize = 10000;
			var rowCount = 0;
			var syncRoot = new object();

			using (var input = new StreamReader(File.OpenRead(path)))
			{
				var header = input.ReadLine(); // Keep the header
				Parallel.ForEach(
					input.ReadLines().TakeChunks(batchSize),
					new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },
					lines =>
					{
						var linesWithHeader = new List<string> { header }
										.Concat(lines);
						var session = Util.GetSession();
						var transaction = Util.BeginTransaction(session);
						var records = Util.ReadFromList<DeviationCell, DeviationCellCsvClassMap>(linesWithHeader);
						foreach (var record in records)
						{
							Interlocked.Increment(ref rowCount);
							session.SaveOrUpdate(record);
						}
						Log($"Processed {rowCount} rows");
						transaction.Commit();
						session.Flush();
					});
				Log($"Done processing {rowCount} deviation cells");
			}
		}
		private static void GenerateConvexHulls(List<GravityVector> gravityVectors, NormalRoute normalRoute)
		{
			normalRoute.NormalRouteStdGeometry = GenerateConvexHull(
					gravityVectors.Select(gv => gv.StdDevLeftPosition.Coordinate).ToList(),
					gravityVectors.Select(gv => gv.StdDevRightPosition.Coordinate).ToList());

			normalRoute.NormalRouteMaxGeometry = GenerateConvexHull(
					gravityVectors.Select(gv => gv.MaxDevLeftPosition.Coordinate).ToList(),
					gravityVectors.Select(gv => gv.MaxDevRightPosition.Coordinate).ToList());
		}

		public static Polygon GenerateConvexHull(List<Coordinate> currentLeftCoords, List<Coordinate> currentRightCoords)
		{
			currentRightCoords.Reverse();
			var combinedCoordsList = currentLeftCoords.Concat(currentRightCoords).ToList();
			combinedCoordsList.Add(combinedCoordsList.First());
			var convexHull = new Polygon(new LinearRing(combinedCoordsList.ToArray()));
			convexHull.SRID = 4326;

			if (!convexHull.IsValid)
			{
				var newGeom = convexHull.Buffer(0);
				if (!(newGeom is Polygon))
				{
					Log($"The fixed geometry became a {newGeom.GetType().Name} :(");
					convexHull = null;
				}
				else if (!newGeom.IsValid)
				{
					Log("Fix unsuccessful :(");
					convexHull = null;
				}
				else
				{
					Log("Geometry was invalid but was successfully fixed!");
					convexHull = newGeom as Polygon;
				}
			}

			return convexHull;
		}
	}
}