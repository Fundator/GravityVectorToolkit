using CommandLineParser.Arguments;
using DemoDataAccess;
using log4net;
using log4net.Config;
using System;
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

            FluentConfiguration.Configure();


            if (pathArg.Parsed)
            {
                var path = Path.GetFullPath(pathArg.Value);

                if (Directory.Exists(path))
                {

                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    int fileCount = files.Count();
                    Log.Debug($"Loading {fileCount} gravity vectors");
                    Log.Debug("Starting..");

                }
            }
        }
    }
}