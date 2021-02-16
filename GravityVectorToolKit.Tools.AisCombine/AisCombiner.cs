using GravityVectorToolKit.Common;
using GravityVectorToolKit.Tools.AisCombine.DataAccess;
using log4net;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NetTopologySuite.Geometries;
using Sylvan.Data.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GravityVectorToolKit.Tools.AisCombine
{
	public class AisCombiner
	{
		private const int statusMessageCycleSeconds = 10;
		private static ILog Log = LogManager.GetLogger(typeof(AisCombiner));

		private AisCombinerSettings Specification;
		private List<SourceFileMetadata> Analysis;
		private EpochManager EpochManager;
		private GvtkGeohasher GeoHasher;
		private HashSet<string> DeadDogs;
		private Object SyncRoot = new Object();

		public AisCombiner(AisCombinerSettings specification)
		{
			Specification = specification;
		}

		#region Public methods

		public void Prepare()
		{
		}

		public void Analyze()
		{
			var files = Directory.GetFiles(Specification.AisSourceDirectory).OrderBy(f => f).ToList();
			var results = new List<SourceFileMetadata>(files.Count());
			var syncRoot = new Object();

			Log.Info($"Analyzing {files.Count()} files");
			Parallel.ForEach(files, file =>
			{
				var lines = File.ReadLines(file);
				var result = new SourceFileMetadata();
				result.Path = file;
				lock (syncRoot)
				{
					results.Add(result);
				}
			});
			Log.Info($"Analysis complete");
			Analysis = results.OrderBy(r => r.Path).ToList();
		}

		public void Run()
		{
			EpochManager = new EpochManager(Specification.WeatherDbPath);
			GeoHasher = new GvtkGeohasher();
			DeadDogs = new HashSet<string>();

			long rowCounter = 0;
			int fileCounter = 0;

			long previousCounter = 0;

			var timer = new System.Timers.Timer(statusMessageCycleSeconds * 1000);
			timer.Elapsed += (a, b) =>
			{
				Log.Info($"Processed {rowCounter} rows, current throughput is {(rowCounter - previousCounter) / statusMessageCycleSeconds} rows per second");
				Log.Info($"The current size of the redirect map is {RedirectMap.Count()} and there are {DeadDogs.Count()} dead dogs");
				previousCounter = rowCounter;
				EpochManager.VacuumPeriodic();
			};
			timer.AutoReset = true;
			timer.Enabled = true;

			Parallel.ForEach(Analysis.AsParallel().AsOrdered(), new ParallelOptions
			{
				MaxDegreeOfParallelism = Specification.MaxParallellism
			},
			analysisResult =>
			{
				string msg;
				var sourceFile = analysisResult.Path;

				try
				{
					msg = ProcessFile(sourceFile, ref rowCounter, ref fileCounter);
				}
				catch (Exception e)
				{
					Log.Error(e);
					// TODO: Retry
				}
				try
				{
					lock (SyncRoot)
					{
						var redirectMapStr = DumpRedirectMap();
						File.WriteAllText("RedirectMap.csv", redirectMapStr);
						Log.Info($"Redirect map has been updated ({redirectMapStr.Split('\n').Length} lines)");
					}
				}
				catch (Exception e)
				{
					Log.Info("Unable to dump redirect map at this time because: " + e.Message);
				}
			});

			Log.Info("Dumping redirect map");
			File.WriteAllText("RedirectMap.csv", DumpRedirectMap());

			Log.Info("Done!");
		}

		#endregion Public methods

		#region Utilities

		private string ProcessFile(string sourceFile, ref long counter, ref int fileCounter)
		{
			string msg;
			lock (SyncRoot)
			{
				Interlocked.Increment(ref fileCounter);
				msg = $"{Path.GetFileName(sourceFile)} {fileCounter} of {Analysis.Count()}";
				Log.Info($"Processing " + msg);
			}
			var csv = CsvDataReader.Create(sourceFile, new CsvDataReaderOptions
			{
				//StringFactory = new StringPool().GetString,
				HasHeaders = true
			});

			var writer = CreateStreamWriter(sourceFile);
			WriteNewHeader(sourceFile, writer);

			long currentRoundedEpoch = 0;
			while (csv.Read())
			{
				var timestamp = csv.GetDateTime(Specification.TimestampColumnName);
				var epoch = timestamp.ToEpoch();
				var roundedEpoch = Util.RoundToHourEpoch(timestamp);

				currentRoundedEpoch = EnsureEpochIsLoaded(currentRoundedEpoch, roundedEpoch);
				var lat = csv.GetFloat(Specification.LatitudeColumnName);
				var lon = csv.GetFloat(Specification.LongitudeColumnName);
				var point = new Point(lon, lat);
				var hash = GeoHasher.Encode(point, Specification.Precision);

				WeatherHist match = null;

				if (!DeadDogs.Contains(hash) && !DeadDogs.Contains(GeoHasher.Reduce(hash, Specification.PrecisionSearchLimit)))
				{
					var matches = EpochManager.Lookup(currentRoundedEpoch, Redirect(hash), false);
					if (matches.Count() == 0)
					{
						DeepSearch(currentRoundedEpoch, hash, matches);
					}
					if (matches.Count() > 0)
					{
						match = SelectBestMatch(point, matches);
						MapRedirect(hash, GeoHasher.Reduce(match.Geohash, Specification.Precision));
					}
				}

				Interlocked.Increment(ref counter);
				AppendWeatherData(csv, writer, match);
			}
			writer.Flush();
			writer.Close();
			writer.Dispose();
			Log.Info("Completed " + msg);
			return msg;
		}

		private WeatherHist SelectBestMatch(Point point, List<WeatherHist> matches)
		{
			WeatherHist match;
			if (!Specification.CalculateClosest)
			{
				match = matches.FirstOrDefault();
			}
			else if (Specification.CalculateClosest && matches.Count > 1)
			{
				match = matches.OrderBy(r => GeoHasher.Decode(r.Geohash).Distance(point)).First();
			}
			else
			{
				match = matches.FirstOrDefault();
			}

			return match;
		}

		private static void AppendWeatherData(CsvDataReader csv, StreamWriter writer, WeatherHist result)
		{
			var rowData = new string[csv.FieldCount + 4];
			csv.GetValues(rowData);

			if (result != null)
			{
				var invariantCulture = CultureInfo.InvariantCulture;
				rowData[rowData.Length - 1] = string.Format(invariantCulture, "{0:0.##}", result.Dd);
				rowData[rowData.Length - 2] = string.Format(invariantCulture, "{0:0.##}", result.Ff);
				rowData[rowData.Length - 3] = string.Format(invariantCulture, "{0:0.##}", result.Thq);
				rowData[rowData.Length - 4] = string.Format(invariantCulture, "{0:0.##}", result.Hs);
			}

			writer.WriteLine(string.Join(';', rowData));
		}

		private long EnsureEpochIsLoaded(long currentRoundedEpoch, long roundedEpoch)
		{
			if (currentRoundedEpoch != roundedEpoch)
			{
				currentRoundedEpoch = roundedEpoch;
			}

			if (!EpochManager.IsAvailable(currentRoundedEpoch))
			{
				if (Specification.AssumeNoEpochCollissions)
				{
					EpochManager.Load(currentRoundedEpoch);
				}
				else
				{
					lock (SyncRoot)
					{
						if (!EpochManager.IsAvailable(currentRoundedEpoch))
						{
							EpochManager.Load(currentRoundedEpoch);
						}
					}
				}
			}

			return currentRoundedEpoch;
		}

		private void DeepSearch(long currentRoundedEpoch, string hash, List<WeatherHist> data)
		{
			var reducedHash = hash;
			while (true)
			{
				var fromNeighbours = FindNeighbours(currentRoundedEpoch, reducedHash);
				var fromCurrentReducedHash = EpochManager.Lookup(currentRoundedEpoch,
													reducedHash,
														reducedHash.Length < Specification.Precision);
				data.AddRange(fromNeighbours);
				data.AddRange(fromCurrentReducedHash);

				if (data.Count() > 0)
				{
					break;
				}

				if (GeoHasher.Reduce(reducedHash).Length == Specification.PrecisionSearchLimit - 1)
				{
					DeadDogs.Add(hash);
					var parents = GeoHasher.Parents(hash, Specification.PrecisionSearchLimit);
					foreach (var parent in parents)
					{
						DeadDogs.Add(parent);
					}

					Log.Info($"Can't find anything for {hash} up until {reducedHash}, giving up");
					break;
				}

				reducedHash = GeoHasher.Reduce(reducedHash);
			}
		}

		private static void WriteNewHeader(string sourceFile, StreamWriter writer)
		{
			var header = File.ReadLines(sourceFile).First().Split(";").ToList();
			header.Add("hs");
			header.Add("thq");
			header.Add("ff");
			header.Add("dd");
			writer.WriteLine(string.Join(';', header.ToArray()));
		}

		private StreamWriter CreateStreamWriter(string sourceFile)
		{
			var outputFileName = Path.Combine(Specification.AisDestinationDirectory, Path.GetFileName(sourceFile) + ".weather.csv");
			var writer = new StreamWriter(outputFileName, false, Encoding.UTF8, 65536); // 4MB buffer
			writer.AutoFlush = true;
			return writer;
		}

		private List<WeatherHist> FindNeighbours(long currentRoundedEpoch, string hash)
		{
			var neighbours = GeoHasher.Neighbours(hash);
			bool expandedSearch = false;
			int previousCount = neighbours.Count();

			var data = new List<WeatherHist>();
			foreach (var n in neighbours)
			{
				var tmp = EpochManager.Lookup(currentRoundedEpoch, n, n.Length < Specification.Precision);
				if (tmp != null)
				{
					data.AddRange(tmp);
				}
			}

			if (expandedSearch)
			{
				Log.Debug($"Increased search area from {previousCount} to {neighbours.Count()} neighbours and found {data.Count()} matches");
			}

			return data;
		}

		private Dictionary<string, string> RedirectMap = new Dictionary<string, string>();

		private string Redirect(string hash)
		{
			if (RedirectMap.ContainsKey(hash))
			{
				return RedirectMap[hash];
			}
			else
			{
				return hash;
			}
		}

		private string MapRedirect(string hash, string redirect)
		{
			return RedirectMap[hash] = redirect;
		}

		private bool HasRedirect(string hash)
		{
			return RedirectMap.ContainsKey(hash);
		}

		private string DumpRedirectMap()
		{
			var b = new StringBuilder();
			var delimiter = ";";
			var hasher = new GvtkGeohasher();
			b.Append("from;to;p_from;p_to;edge;bb_from;bb_to");
			b.Append(Environment.NewLine);
			foreach (var kvp in RedirectMap
									.ToList() // Avoid thread errors
									.Where(kvp => kvp.Key != kvp.Value))
			{
				b.Append(kvp.Key + delimiter);
				b.Append(kvp.Value + delimiter);
				b.Append(hasher.Decode(kvp.Key).AsText() + delimiter);
				b.Append(hasher.Decode(kvp.Value).AsText() + delimiter);
				b.Append(new LineString(new Coordinate[] { hasher.Decode(kvp.Key).Coordinate, hasher.Decode(kvp.Value).Coordinate }).AsText() + delimiter);
				b.Append(hasher.BoundingBox(kvp.Key).AsText() + delimiter);
				b.Append(hasher.BoundingBox(kvp.Value).AsText() + delimiter);
				b.Append(Environment.NewLine);
			}
			return b.ToString();
		}

		#endregion Utilities
	}
}