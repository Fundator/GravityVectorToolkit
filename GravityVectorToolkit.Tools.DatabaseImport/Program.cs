using CommandLineParser.Arguments;
using DemoDataAccess;
using GravityVector.Common;
using GravityVectorToKML.Model;
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

			parser.Arguments.Add(dropAndCreateArg);
			parser.Arguments.Add(pathArg);
			parser.ParseCommandLine(args);

			Log.Debug("Configuring database..");

			if (pathArg.Parsed)
			{
				var path = Path.GetFullPath(pathArg.Value);

				if (Directory.Exists(path))
				{
					bool dropAndCreate = dropAndCreateArg.Parsed ? dropAndCreateArg.Value : true;
					FluentConfiguration.Configure(dropAndCreate);
					var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
					List<ISession> sessions = new List<ISession>();
					int fileCount = files.Count();
					Log.Debug($"Drop and create? " + (dropAndCreate ? "Yes" : "No"));

					if (dropAndCreate)
					{
						Log.Info("You have specified that the schema should be dropped and recreated. Press ctrl-c now to cancel this program.");
						Thread.Sleep(5000);
					}

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

							var stuff = filename.Split(new string[] { "_", "." }, StringSplitOptions.RemoveEmptyEntries);
							int fromLocationId = Int32.Parse(stuff[1]);
							int toLocationId = Int32.Parse(stuff[2]);

							// If the user opted to keep existing data, then we need to check if the current row exist
							// This puts more load on the database, but can potentially save a lot of time
							bool skip = false;
							if (!dropAndCreate)
							{
								var result = session.Query<NormalPointG>()
													.Where(p => p.FromLocationId == fromLocationId 
															&& p.ToLocationId == toLocationId).Count();
								skip = result > 0;
							}

							if (!skip)
							{
								List<NormalPointG> records = Util.ReadGravityVector(file);
								accRecords += (ulong)(records.Count);

								ITransaction transaction = BeginTransaction(session);
								foreach (var record in records)
								{
									record.FromLocationId = fromLocationId;
									record.ToLocationId = toLocationId;
									batchRecords++;
									session.SaveOrUpdate(record);
								}


								lock (syncRoot) // Ensure flushing is always done in sync
								{
									Log.Info($"Committing transaction and flushing session for file {filename}");
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
				session.FlushMode = FlushMode.Never;
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
				catch (Exception e)
				{
					if (i == n)
					{
						throw e;
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