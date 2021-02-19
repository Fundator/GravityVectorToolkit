using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLineParser.Arguments;

namespace GravityVectorToolKit.Tools.AisCombine
{
	internal static class Program
	{
		private static ILog Log = LogManager.GetLogger(typeof(AisCombiner));

		private static void Main(string[] args)
		{
			ConfigureLogging();
			Log.Info(@" __  __   _   ___   _   ___ _____ ");
			Log.Info(@"|  \/  | /_\ |   \ /_\ | _ \_   _|");
			Log.Info(@"| |\/| |/ _ \| |) / _ \|   / | |  ");
			Log.Info(@"|_|  |_/_/ \_\___/_/ \_\_|_\ |_|  ");
			Log.Info(@"----------------------------------");
			Log.Info("AIS Combine " + Assembly.GetEntryAssembly().GetName().Version);
			Log.Info(@"----------------------------------");

			var settingsFilePath = GetSettingsFilePathFromCmdLineArgs(args);
			AisCombinerSettings spec;
			if (File.Exists(settingsFilePath))
			{
				spec = JsonConvert.DeserializeObject<AisCombinerSettings>(File.ReadAllText(settingsFilePath));
			}
			else
			{
				Log.Error("settings.json could not be found");
				return;
			}

			Log.Info("Validating..");
			var errors = spec.Validate();
			if (errors.Any())
			{
				foreach (var error in errors)
				{
					Log.Error(error);
				}
				return;
			}
			Log.Info("Validation passed");
			var combiner = new AisCombiner(spec);
			Log.Info("Preparing..");

			combiner.Analyze();
			combiner.Prepare();
			combiner.Run();
		}

		private static string GetSettingsFilePathFromCmdLineArgs(string[] args)
		{
			string settingsFilePath = "settings.json";
			var parser = new CommandLineParser.CommandLineParser();
			var fileArgument = new FileArgument('f', "settings");
			fileArgument.FileMustExist = true;
			parser.Arguments.Add(fileArgument);
			parser.ParseCommandLine(args);
			if (fileArgument.Parsed)
			{
				settingsFilePath = fileArgument.Value.FullName;
			}

			return settingsFilePath;
		}

		private static void ConfigureLogging()
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
		}
	}
}