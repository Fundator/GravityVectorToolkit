using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;

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


			AisCombinerSettings spec;
			if (File.Exists("settings.json"))
			{
				spec = JsonConvert.DeserializeObject<AisCombinerSettings>(File.ReadAllText("settings.json"));
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

		private static void ConfigureLogging()
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
		}
	}
}