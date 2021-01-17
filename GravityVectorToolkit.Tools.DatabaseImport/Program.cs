using CommandLineParser.Arguments;
using GravityVectorToolKit.DataAccess;
using GravityVectorToolKit.DataModel;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace GravityVectorToolkit.Tools.DatabaseImport
{
	public static partial class Program
	{
		private static Object syncRoot = new Object();

		private static ILog Log = LogManager.GetLogger(typeof(Program));

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
				'g', "gv-merged-path", "Path to a folder containing gravity vector files");

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

			if (normalRoutePathArg.Parsed)
			{
				var path = Path.GetFullPath(normalRoutePathArg.Value);

				if (File.Exists(path))
				{
					normalRoutes = DatabaseImport.ImportNormalRoutes(path);
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
					DatabaseImport.ImportGravityVectors(syncRoot, path, normalRoutes);
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

			if (deviationCellsPathArg.Parsed)
			{
				var path = Path.GetFullPath(deviationCellsPathArg.Value);

				if (File.Exists(path))
				{
					DatabaseImport.ImportDeviationCells(path);
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
		}
	}
}