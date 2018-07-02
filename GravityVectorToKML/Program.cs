using CommandLineParser.Arguments;
using CsvHelper;
using CsvHelper.Configuration;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GravityVectorToKML
{
    static class Program
    {
        static void Main(string[] args)
        {

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
                    Console.WriteLine($"Loading {files.Count()} gravity vectors");
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

                            var csv = new CsvReader(File.OpenText(file), new Configuration
                            {
                                CultureInfo = CultureInfo.InvariantCulture
                            });
                            var records = csv.GetRecords<NormalPoint>().ToList();
                            Console.WriteLine($"Processing {Path.GetFileName(kmlFileName)}: {records.Count()} records");


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
                                if (currentClusterIndex != record.clusterindex)
                                {
                                    currentClusterIndex = record.clusterindex;
                                    folder = new Folder();
                                    folder.Name = "Cluster " + currentClusterIndex.ToString();
                                    rootFolder.AddFeature(folder);
                                    newCluster = true;
                                }

                                if (!newCluster)
                                {
                                    clusterSegment.Coordinates.Add(new Vector(previousNormalPoint.x, previousNormalPoint.y));
                                }
                                clusterSegment.Coordinates.Add(new Vector(record.x, record.y));

                                Placemark placemark = new Placemark();
                                placemark.AddStyle(new Style()
                                {
                                    Line = new LineStyle()
                                    {
                                        PhysicalWidth = record.dist_med,//Math.Min(record.dist_med / 150, 1) * 100,
                                        Color = GetColorFromSpeed(record.sog),
                                    }
                                });
                                placemark.Geometry = clusterSegment;
                                placemark.Name = $"Grid: {record.grid_id} / Distance: {record.dist_med} / Speed: {record.sog} / Course: {record.cog}";
                                folder.AddFeature(placemark);

                                // Do this last!
                                previousNormalPoint = record;
                                previousClusterSegment = clusterSegment;
                            }

                            kml.Feature = rootFolder;
                            serializer.Serialize(kml);
                            File.WriteAllText(kmlFileName, serializer.Xml);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                            Console.WriteLine("Stack trace: " + e.StackTrace);
                        }

                    });
                }

                else
                {
                    Console.WriteLine("Path not found: " + path);
                }
            }
            else
            {
                Console.WriteLine("You must specify --path");
            }

        }

        private static Color32 GetColorFromSpeed(double speed)
        {
            var normalizedSpeed = Math.Min(speed / 13, 1);
            Color32 color32 = new Color32(255, 0, Convert.ToByte((int)(255 * (1 - normalizedSpeed))), Convert.ToByte((int)(normalizedSpeed * 255)));
            return color32;

        }

        public class NormalPoint
        {
            public double clusterindex { get; set; }
            public double grid_id { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double sog { get; set; }
            public double cog { get; set; }
            public double dist_med { get; set; }
            public string max_dist_left { get; set; }
            public string max_dist_right { get; set; }
            public string dist_std_left { get; set; }
            public string dist_std_right { get; set; }
            public double max_lesser_sog_diff { get; set; }
            public double max_greater_sog_diff { get; set; }
            public double lesser_sog_std { get; set; }
            public double greater_sog_std { get; set; }
            public string max_lesser_cog_diff { get; set; }
            public string max_greater_cog_diff { get; set; }
            public double lesser_cog_std { get; set; }
            public double greater_cog_std { get; set; }
            public double data_count { get; set; }
        }

        public class ModelClassMap : ClassMap<NormalPoint>
        {
            public ModelClassMap()
            {
                Map(m => m.clusterindex).Name("clusterindex");
                Map(m => m.grid_id).Name("grid_id");
                Map(m => m.x).Name("x");
                Map(m => m.y).Name("y");
                Map(m => m.sog).Name("sog");
                Map(m => m.cog).Name("cog");
                Map(m => m.dist_med).Name("dist_med");
                Map(m => m.max_dist_left).Name("max_dist_left");
                Map(m => m.max_dist_right).Name("max_dist_right");
                Map(m => m.dist_std_left).Name("dist_std_left");
                Map(m => m.dist_std_right).Name("dist_std_right");
                Map(m => m.max_lesser_sog_diff).Name("max_lesser_sog_diff");
                Map(m => m.max_greater_sog_diff).Name("max_greater_sog_diff");
                Map(m => m.lesser_sog_std).Name("lesser_sog_std");
                Map(m => m.greater_sog_std).Name("greater_sog_std");
                Map(m => m.max_lesser_cog_diff).Name("max_lesser_cog_diff");
                Map(m => m.max_greater_cog_diff).Name("max_greater_cog_diff");
                Map(m => m.lesser_cog_std).Name("lesser_cog_std");
                Map(m => m.greater_cog_std).Name("greater_cog_std");
                Map(m => m.data_count).Name("data_count");
            }
        }

    }
}
