using CommandLineParser.Arguments;
using GravityVectorToolKit.Common;
using GravityVectorToolKit.Common.Extensions;
using GravityVectorToolKit.CSV.Mapping;
using GravityVectorToolKit.DataAccess;
using GravityVectorToolKit.DataModel;
using log4net;
using log4net.Config;
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
		private static Object syncRoot = new Object();

		private static void Main(string[] args)
		{
			ConfigureLog();
			Run(args);
		}

		private static void ConfigureLog()
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
		}

		private static void Run(string[] args)
		{
			var parser = new CommandLineParser.CommandLineParser();

			var connectionStringArg = new ValueArgument<string>(
				'c', "connectionstring", "Connection string to the PostgreSQL database");

			var dropAndCreateArg = new ValueArgument<bool>(
				'd', "drop", "Drop and recreate the database");

			var pathArg = new ValueArgument<string>(
				'p', "gv-folder-path", "Path to a folder containing gravity vector files");

			var mergedGravityVectorFilePath = new ValueArgument<string>(
				'g', "gv-merged-path", "Path to a merged file containing gravity vectors");

			var normalRoutePathArg = new ValueArgument<string>(
				'n', "normal-route-path", "Path to a file containing normal route linestrings");

			var deviationCellsPathArg = new ValueArgument<string>(
				'm', "deviation-cells-path", "Path to a file containing deviation map cells");

			parser.Arguments.Add(connectionStringArg);
			parser.Arguments.Add(dropAndCreateArg);
			parser.Arguments.Add(pathArg);
			parser.Arguments.Add(normalRoutePathArg);
			parser.Arguments.Add(deviationCellsPathArg);
			parser.ParseCommandLine(args);

			bool dropAndCreate = dropAndCreateArg.Parsed ? dropAndCreateArg.Value : true;
			if (dropAndCreate)
			{
				Log.Info("You have specified that the schema should be dropped and recreated. Press ctrl-c now to cancel this program.");
				Thread.Sleep(5000);
			}

			var connectionString = connectionStringArg.Value;
;
			Log.Info("Configuring database..");
			FluentConfiguration.Configure(connectionString, dropAndCreate);
			Log.Info("Database connected!");

			List<NormalRoute> normalRoutes = null;

			if (deviationCellsPathArg.Parsed)
			{
				var path = Path.GetFullPath(deviationCellsPathArg.Value);

				if (File.Exists(path))
				{
					LoadDeviationCells(path);
				}
				else
				{
					Log.Error($"The deviation map path {path} does not exist");
					return;
				}
			}
			else
			{
				Log.Info("Deviation cell file not specified, skipping..");
			}

			if (normalRoutePathArg.Parsed)
			{
				var path = Path.GetFullPath(normalRoutePathArg.Value);

				if (File.Exists(path))
				{
					normalRoutes = LoadNormalRoutes(path);
				}
				else
				{
					Log.Error($"The path {path} does not exist");
					return;
				}
			}
			else
			{
				if (pathArg.Parsed)
				{
					Log.Error("You have not specified a normal route file. Press ctrl-c now to cancel this program, or wait to attempt loading gravity vectors.");
				}
				else
				{
					Log.Info("Normal route path not specified, skipping..");
				}

				Thread.Sleep(5000);
			}

			if (pathArg.Parsed)
			{
				var path = Path.GetFullPath(pathArg.Value);

				if (File.Exists(path))
				{
					LoadGravityVectors(syncRoot, path, normalRoutes);
				}
				else
				{
					Log.Error($"The file {path} does not exist");
					return;
				}
			}
			else
			{
				Log.Info($"Gravity vector path not specified, skipping..");
			}
		}

		private static void LoadDeviationCells(string path)
		{
			var batchSize = 10000;
			var rowCount = 0;
			var syncRoot = new object();

			using (var input = new StreamReader(File.OpenRead(path)))
			{
				var header = input.ReadLine(); // Keep the header
				Parallel.ForEach(
					input.ReadLines().TakeChunks(batchSize),
					new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount /* better be number of CPU cores */ },
					lines =>
					{
						var linesWithHeader = new List<string> { header }
										.Concat(lines);

						var session = Util.GetSession();
						var transaction = Util.BeginTransaction(session);
						var records = Util.ReadFromList<DeviationCell, DeviationCellCsvClassMap>(linesWithHeader);

						foreach (var record in records)
						{
							rowCount++;
							if (rowCount % 1000 == 0)
							{
								Log.Info($"Processed {rowCount} rows");
							}
							session.SaveOrUpdate(record);
						}
						transaction.Commit();
						session.Flush();
					});
			}
		}

		private static void LoadGravityVectors(object syncRoot, string file, List<NormalRoute> normalRoutes)
		{
			Log.Debug($"Loading gravity vectors..");

			var normalRouteMap = normalRoutes.ToDictionary(x => x.NormalRouteId, x => x);

			try
			{
				using (var input = new StreamReader(File.OpenRead(file)))
				{
					// Keep the header
					var header = input.ReadLine();

					var filename = Path.GetFileName(file);

					int rowCount = 0;
					Log.Info($"Loading gravity vectors from {Path.GetFileName(file)}..");

					const int batchSize = 10000;
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

								var normalRoute = normalRouteMap[gravityVector.NormalRouteId];
								gravityVector.NormalRoute = normalRoute;
								if (normalRoute.GravityVectors == null)
								{
									normalRoute.GravityVectors = new List<GravityVector>();
								}
								normalRoute.GravityVectors.Add(gravityVector);
								session.SaveOrUpdate(gravityVector);
							}

							Log.Info($"Processed {rowCount} rows..");

							transaction.Commit();
							session.Flush();
							session.Close();
							session.Dispose();
						}
						catch (Exception e)
						{
							Log.Error(e);
						}
					});
					Log.Info($"Done processing {rowCount} records");
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	

		private static List<NormalRoute> LoadNormalRoutes(string path)
		{
			List<NormalRoute> allNormalRoutes = new List<NormalRoute>();
			var filename = Path.GetFileName(path);
			Log.Info($"Loading normal routes from {filename}..");
			//normalRoutes = Util.ReadCsvFile<NormalRoute, NormalRouteCsvClassMap>(path);
			var syncRoot = new object();
			const int batchSize = 150;
			int rowCount = 0;
			using (var input = new StreamReader(File.OpenRead(path)))
			{
				var header = input.ReadLine(); // Keep the header
				Parallel.ForEach(
					input.ReadLines().TakeChunks(batchSize),
					new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount /* better be number of CPU cores */ },
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
								Log.Info($"Processed {rowCount} normal routes");
							}
						}
						transaction.Commit();
						nrSession.Flush();
						nrSession.Close();
						lock(syncRoot)
						{
							allNormalRoutes.AddRange(normalRoutes);
						}
					});
			}
			Log.Info($"Done processing {rowCount} normal routes");

			return allNormalRoutes;
		}
	}
}