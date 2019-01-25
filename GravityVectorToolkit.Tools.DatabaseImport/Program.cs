using CommandLineParser.Arguments;
using DemoDataAccess;
using GeoAPI.Geometries;
using GravityVector.Common;
using GravityVectorToolKit.DataModel;
using log4net;
using log4net.Config;
using NetTopologySuite.Geometries;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GravityVectorToolkit.Tools.DatabaseImport
{
	internal static class Program
	{
		private static ILog Log = LogManager.GetLogger(typeof(Program));

		private static void Main(string[] args)
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
			var syncRoot = new Object();

			CommandLineParser.CommandLineParser parser =
				new CommandLineParser.CommandLineParser();

			ValueArgument<bool> dropAndCreateArg = new ValueArgument<bool>(
				'd', "drop", "Drop and recreate the database");

			ValueArgument<string> pathArg = new ValueArgument<string>(
				'p', "path", "Path to a folder containing gravity vector files");

			ValueArgument<string> normalRoutePathArg = new ValueArgument<string>(
				'n', "normal-route-path", "Path to a file containing normal route linestrings");

			parser.Arguments.Add(dropAndCreateArg);
			parser.Arguments.Add(pathArg);
			parser.Arguments.Add(normalRoutePathArg);
			parser.ParseCommandLine(args);

			Log.Debug("Configuring database..");

			bool dropAndCreate = dropAndCreateArg.Parsed ? dropAndCreateArg.Value : true;
			if (dropAndCreate)
			{
				Log.Info("You have specified that the schema should be dropped and recreated. Press ctrl-c now to cancel this program.");
				Thread.Sleep(5000);
			}

			FluentConfiguration.Configure(dropAndCreate);


			Dictionary<string, NormalRoute> normalRouteLookup = null;
			List<NormalRoute> normalRoutes = null;

			if (normalRoutePathArg.Parsed)
			{
				var path = Path.GetFullPath(normalRoutePathArg.Value);

				if (File.Exists(path))
				{
					Log.Info("Loading normal routes..");
					normalRoutes = Util.ReadNormalRouteFile(path);
					//Log.Info($"{normalRoutes.Count} normal routes loaded, creating lookup table..");
					//foreach (var normalRoute in normalRoutes)
					//{
					//	normalRouteLookup[normalRoute.FromLocationId + "-" + normalRoute.ToLocationId] = normalRoute;
					//}

					ISession nrSession = GetSession();
					ITransaction transaction = BeginTransaction(nrSession);
					foreach (var normalRoute in normalRoutes)
					{
						nrSession.SaveOrUpdate(normalRoute);
					}
					Log.Info($"Saving normal routes to database..");
					transaction.Commit();
					nrSession.Flush();
					nrSession.Close();
				}
				else
				{
					Log.Error($"The path {path} does not exist");
					return;
				}
			}
			else
			{
				Log.Warn("You have not specified a normal route file. Press ctrl-c now to cancel this program.");
				Thread.Sleep(5000);
			}

			if (pathArg.Parsed)
			{
				var path = Path.GetFullPath(pathArg.Value);

				if (Directory.Exists(path))
				{
					var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
					List<ISession> sessions = new List<ISession>();
					int fileCount = files.Count();
					Log.Debug($"Drop and create? " + (dropAndCreate ? "Yes" : "No"));








					Log.Debug($"Loading {fileCount} gravity vectors");
					Log.Debug("Starting..");

					int processedFiles = 0;
					ulong accRecords = 0;
					Parallel.ForEach(files, file =>
					{
						try
						{
							// Maintain a session per thread
							ISession session = GetSession();

							var filename = Path.GetFileName(file);

							ulong batchRecords = 0;

							var stuff = filename.Split(new string[] { "_", ".", "-" }, StringSplitOptions.RemoveEmptyEntries);
							int fromLocationId = Int32.Parse(stuff[4]);
							int toLocationId = Int32.Parse(stuff[5]);

							// If the user opted to keep existing data, then we need to check if the current row exist
							// This puts more load on the database, but can potentially save a lot of time
							bool skip = false;
							if (!dropAndCreate)
							{
								var result = session.Query<NormalRoute>()
													.Where(p => p.FromLocationId == fromLocationId
															&& p.ToLocationId == toLocationId).Count();
								skip = result > 0;
							}

							if (!skip)
							{
								List<NormalPoint> records = Util.ReadGravityVector(file);
								accRecords += (ulong)(records.Count);

								//NormalRoute normalRoute = new NormalRoute();

								//var normalRouteKey = fromLocationId + "-" + toLocationId;
								//if (normalRouteLookup != null && normalRouteLookup.ContainsKey(normalRouteKey))
								//{
								//	normalRoute = normalRouteLookup[normalRouteKey];
								//}
								//else
								//{
								//	normalRoute = new NormalRoute();
								//	normalRoute.FromLocationId = fromLocationId;
								//	normalRoute.ToLocationId = toLocationId;
								//}

								//MapRoutePath(normalRoute, records);

								//normalRoute.NormalPoints = records;
								//foreach (var record in records)
								//{
								//	record.NormalRoute = normalRoute;
								//	batchRecords++;
								//}

								ITransaction transaction = BeginTransaction(session);
								foreach(var normalPoint in records)
								{
									session.SaveOrUpdate(normalPoint);
									batchRecords++;
								}

								lock (syncRoot) // Ensure flushing is always done in sync
								{
									Log.Debug($"Committing transaction and flushing session for file {filename}");
									transaction.Commit();
									session.Flush();
								}
								records = null;
							}
							else
							{
								Log.Info($"Skipped {filename} because it has already been processed");
							}
							session.Close();
							session.Dispose();

							processedFiles++;
							Log.Info($"Processed {batchRecords} records from {filename} ({processedFiles}/{fileCount} files / {((double)processedFiles / (double)fileCount).ToString("0.00%")}) from a preliminary total of {accRecords} records");

						}
						catch (Exception e)
						{
							Log.Error(e);
							Console.WriteLine("Press any key to continue loading gravity vectors..");
							Console.ReadKey();
						}
					});
				}
				else
				{
					Log.Error($"The path {path} does not exist");
					return;
				}
			}
			else
			{
				Log.Error($"You must specify a path");
			}
		}

		/*
		private static void MapRoutePath(NormalRoute normalRoute, List<NormalPoint> normalPoints)
		{
			var coordinateLists = new List<CoordinateList>();
			var noInfiniteLoops = new Dictionary<int, int>();

			Log.Info("Loading gravity vectors into hash table..");
			var gravityVectorLookup = new Dictionary<int, NormalPoint>();

			foreach(var normalPoint in normalPoints)
			{
				gravityVectorLookup[normalPoint.GravityVectorId] = normalPoint;
			}

			foreach (var normalPoint in normalPoints)
			{
				coordinateLists.Add(GetCoordinateListFromNormalPoint(gravityVectorLookup, normalPoints, normalPoint, null, new Dictionary<long, int>()));
			}

			Log.Info("Found " + coordinateLists.Where(cl => cl.Count() >= 2).Count() + " linestrings with 2 or more coordinates.");
			Log.Info("Discarded " + coordinateLists.Where(cl => cl.Count() < 2).Count() + " linestrings with less than 2 coordinates.");

			normalRoute.NormalRouteGeometry = new MultiLineString(coordinateLists.Where(cl => cl.Count() >= 2).Select(cl => new LineString(cl.ToCoordinateArray())).ToArray());
		}
		*/

		/*
		private static CoordinateList GetCoordinateListFromNormalPoint(Dictionary<int, NormalPoint> gravityVectorLookup, List<NormalPoint> normalPoints, NormalPoint currentNormalPoint, CoordinateList coordinates = null, Dictionary<long, int> noInfiniteLoops = null, int depth = 0)
		{
			if (coordinates == null)
			{
				coordinates = new CoordinateList();
			}

			if (noInfiniteLoops == null)
			{
				Log.Warn("Dictionary for tracking circular references is null. This is probably going to take a lot of time.");
				noInfiniteLoops = new Dictionary<long, int>();
			}

			if (currentNormalPoint.SpeedOverGround < 60 && currentNormalPoint.DistanceMedian < 600) // Ignore gravity vectors where the speed is too high
			{
				coordinates.Add(new Coordinate(currentNormalPoint.Longitude, currentNormalPoint.Latitude));
			}

			if (currentNormalPoint.NextGravityVectors.Any())
			{
				foreach(var nextNormalPointId in currentNormalPoint.NextGravityVectors)
				{
					var lookupId = UniqueId(currentNormalPoint.GravityVectorId, nextNormalPointId);
					int vertexLoopCount = 0;
					noInfiniteLoops.TryGetValue(lookupId, out vertexLoopCount);

					if (vertexLoopCount >= 1)
					{
						break;
					}
					else
					{
						var nextNormalPoint = gravityVectorLookup[nextNormalPointId];

						noInfiniteLoops[lookupId] = ++vertexLoopCount;

						GetCoordinateListFromNormalPoint(gravityVectorLookup, normalPoints, nextNormalPoint, coordinates, noInfiniteLoops, ++depth);
					}

				}

			}

			return coordinates;

		}
		*/

		private static long UniqueId(int left, int right)
		{
			long uniqueId = (long)left;
			uniqueId = uniqueId << 32;
			uniqueId += (long)right;
			return uniqueId;
		}

		private static ITransaction BeginTransaction(ISession session)
		{
			return TryNTimes(() =>
			{
				return session.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
			}, 10);
		}

		private static ISession GetSession()
		{
			return TryNTimes(() =>
			{
				var session = SessionManager.SessionFactory.OpenSession();
				session.FlushMode = FlushMode.Manual;
				return session;
			}, 10);
		}

		private static T TryNTimes<T>(Func<T> f, int n)
		{
			int i = 0;
			while (true)
			{
				try
				{
					return f();
				}
				catch (Exception)
				{
					if (i == n)
					{
						throw;
					}
					else
					{
						Log.Warn($"Caught exception, retrying {i}/{n} times..");
						i++;
					}
				}
			}
		}
	}
}