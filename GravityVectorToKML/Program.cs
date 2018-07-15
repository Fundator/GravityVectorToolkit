using CommandLineParser.Arguments;
using CsvHelper;
using CsvHelper.Configuration;
using GravityVectorToKML.Model;
using GravityVectorToKML.CSV.Mapping;
using log4net;
using log4net.Config;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DemoDataAccess;
using System.Collections.Generic;
using GravityVector.Common;

namespace GravityVectorToKML
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

            ValueArgument<string> pathArg = new ValueArgument<string>(
                'p', "path", "Path to a folder containing gravity vector files");

            ValueArgument<string> outputArg = new ValueArgument<string>(
                'o', "output", "Path to a folder for output KML files");

            parser.Arguments.Add(pathArg);
            parser.Arguments.Add(outputArg);
            parser.ParseCommandLine(args);

            if (pathArg.Parsed)
            {
                var path = Path.GetFullPath(pathArg.Value);
                var outputPath = Path.GetFullPath(outputArg.Value);

                if (Directory.Exists(path))
                {

                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    int fileCount = files.Count();
                    Log.Debug($"Loading {fileCount} gravity vectors");
                    Log.Debug("Starting..");

                    int processedFiles = 0;
                    ulong accRecords = 0;
                    Parallel.ForEach(files, file =>
                    {
                        try
                        {
                            string kmlFileName = Path.Combine(outputPath, Path.GetFileName(file) + ".kml");
                            Serializer serializer = new Serializer();
                            Kml kml = new Kml();
                            var currentClusterIndex = -1d;
                            NormalPoint previousNormalPoint = null;
                            LineString previousClusterSegment = null;
                            Folder rootFolder = new Folder();
                            Folder folder = null;

                            List<NormalPoint> records = Util.ReadGravityVector(file);
                            accRecords += (ulong)(records.Count);
                            //Log.Debug($"Processing {Path.GetFileName(kmlFileName)}: {records.Count()} records");


                            foreach (var record in records)
                            {
                                // Skip the first record
                                if (previousNormalPoint == null)
                                {
                                    previousNormalPoint = record;
                                    continue;
                                }

                                LineString clusterSegment = new LineString();
                                clusterSegment.AltitudeMode = AltitudeMode.ClampToGround;
                                clusterSegment.Coordinates = new CoordinateCollection();

                                bool newCluster = false;
                                if (currentClusterIndex != record.ClusterIndex)
                                {
                                    currentClusterIndex = record.ClusterIndex;
                                    folder = new Folder();
                                    folder.Name = "Cluster " + currentClusterIndex.ToString();
                                    rootFolder.AddFeature(folder);
                                    newCluster = true;
                                }

                                if (!newCluster)
                                {
                                    clusterSegment.Coordinates.Add(new Vector(previousNormalPoint.Latitude, previousNormalPoint.Longitude));
                                }
                                clusterSegment.Coordinates.Add(new Vector(record.Latitude, record.Longitude));

                                Placemark placemark = new Placemark();
                                placemark.AddStyle(new Style()
                                {
                                    Line = new LineStyle()
                                    {
                                        PhysicalWidth = record.DistanceMedian,
                                        Color = GetColorFromSpeed(record.SpeedOverGround),
                                    }
                                });
                                placemark.Geometry = clusterSegment;
                                placemark.Name = $"Grid: {record.GridId} / Distance: {record.DistanceMedian} / Speed: {record.SpeedOverGround} / Course: {record.CourseOverGround}";
                                folder.AddFeature(placemark);

                                // Do this last!
                                previousNormalPoint = record;
                                previousClusterSegment = clusterSegment;
                            }

                            kml.Feature = rootFolder;
                            serializer.Serialize(kml);
                            File.WriteAllText(kmlFileName, serializer.Xml);
                            processedFiles++;

                        }
                        catch (Exception e)
                        {
                            Log.Error(e);
                        }

                        if (processedFiles % 5 == 0)
                        {
                            Log.Debug($"Processed {processedFiles}/{fileCount} files ({((double)processedFiles / (double)fileCount).ToString("0.00%")}) for a total of {accRecords} records");
                        }

                    });
                }

                else
                {
                    Log.Debug("Path not found: " + path);
                }
            }
            else
            {
                Log.Debug("You must specify --path");
            }
            Log.Debug("Done!");
        }

        private static Color32 GetColorFromSpeed(double speed)
        {
            var normalizedSpeed = Math.Min(speed / 13, 1);
            Color32 color32 = new Color32(191, 0, Convert.ToByte((int)(255 * (1 - normalizedSpeed))), Convert.ToByte((int)(normalizedSpeed * 255)));
            return color32;

        }

    }
}
