﻿using CommandLineParser.Arguments;
using DemoDataAccess;
using GeoAPI.Geometries;
using GravityVector.Common;
using GravityVectorToKML.Model;
using log4net;
using log4net.Config;
using NetTopologySuite.Geometries;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GravityVectorToolkit.Tools.DatabaseImport
{
	static class Program
	{
		static ILog Log = LogManager.GetLogger(typeof(Program));

		static void Main(string[] args)
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

			FluentConfiguration.Configure(false);


			if (pathArg.Parsed)
			{
				var path = Path.GetFullPath(pathArg.Value);

				if (Directory.Exists(path))
				{

					var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
					List<ISession> sessions = new List<ISession>();
					int fileCount = files.Count();
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
							ITransaction transaction = BeginTransaction(session);

							//sessions.Add(session);
							var filename = Path.GetFileName(file);

							ulong batchRecords = 0;

							var currentClusterIndex = -1d;
							NormalPoint previousNormalPoint = null;

							List<NormalPointG> records = Util.ReadGravityVector(file);
							accRecords += (ulong)(records.Count);
							var stuff = filename.Split(new string[] { "_", "." }, StringSplitOptions.RemoveEmptyEntries);
							int fromLocationId = Int32.Parse(stuff[1]);
							int toLocationId = Int32.Parse(stuff[2]);

							foreach (var record in records)
							{
								record.FromLocationId = fromLocationId;
								record.ToLocationId = toLocationId;
								batchRecords++;
								session.SaveOrUpdate(record);
							}

							processedFiles++;
							Log.Info($"Processed {batchRecords} records from {filename} ({processedFiles}/{fileCount} files / {((double)processedFiles / (double)fileCount).ToString("0.00%")}) from a preliminary total of {accRecords} records");

							lock (syncRoot) // Ensure flushing is always done in sync
							{
								Log.Info($"Committing transaction and flushing session for file {filename}");
								transaction.Commit();
								session.Flush();
								session.Close();
								session.Dispose();
							}

						}
						catch (Exception e)
						{
							Log.Error(e);
							Console.WriteLine("Press any key to continue loading gravity vectors..");
							Console.ReadKey();
						}
					});
				}
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