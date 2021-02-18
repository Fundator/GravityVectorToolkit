using GravityVectorToolKit.Common;
using GravityVectorToolKit.Tools.AisCombine.DataAccess;
using log4net;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Sylvan;
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
		
		private static ILog Log = LogManager.GetLogger(typeof(AisCombiner));

		private AisCombinerSettings Settings;
		private List<SourceFileMetadata> Analysis;
		private EpochManager EpochManager;
		private GvtkGeohasher GeoHasher;
		private HashSet<string> DeadDogs;
		private Object SyncRoot = new Object();
		private long MaximumEpoch;
		private long MinimumEpoch;

		public AisCombiner(AisCombinerSettings settings)
		{
			Settings = settings;
		}

		#region Public methods

		public void Prepare()
		{
			Log.Info("Inspecting database..");
			var madartWeatherDbContext = new MadartWeatherDbContext(Settings.WeatherDbPath);
			MaximumEpoch = madartWeatherDbContext.WeatherHists.Max(wh => wh.Epoch);
			MinimumEpoch = madartWeatherDbContext.WeatherHists.Min(wh => wh.Epoch);
			Log.Info($"The database contains weather data from {Util.DateTimeFromEpoch(MinimumEpoch)} to {Util.DateTimeFromEpoch(MaximumEpoch)}");
			Log.Info("AIS position outside this range will be skipped silently");
		}

		public void Analyze()
		{
			var files = Directory.GetFiles(Settings.AisSourceDirectory).OrderBy(f => f).ToList();
			var results = new List<SourceFileMetadata>(files.Count());
			var syncRoot = new Object();

			Log.Info($"Analyzing {files.Count()} files");
			Parallel.ForEach(files, file =>
			{
				//var lines = File.ReadLines(file);
				var result = new SourceFileMetadata();
				result.FileSize = new FileInfo(file).Length;
				result.Path = file;
				lock (syncRoot)
				{
					results.Add(result);
				}
			});
			Log.Info($"Analysis complete");
			Log.Info($"Total size: {Util.FormatFileSize(results.Sum(r=>r.FileSize))}");
			if (Settings.SortBy == "path")
			{
				Analysis = results.OrderBy(r => r.Path).ToList();
			}
			else if (Settings.SortBy == "size")
			{
				Analysis = results.OrderByDescending(r => r.FileSize).ToList();
			}

		}

		public void Run()
		{
			EpochManager = new EpochManager(Settings.WeatherDbPath, Settings.MaximumEpochAgeMinutes);
			GeoHasher = new GvtkGeohasher();
			DeadDogs = new HashSet<string>();

			long rowCounter = 0;
			int fileCounter = 0;

			long previousCounter = 0;

			var timer = new System.Timers.Timer(Settings.StatusMessageCycleSeconds * 1000);
			timer.Elapsed += (a, b) =>
			{
				Log.Info($"Files: {fileCounter} - Rows: {rowCounter} - {(rowCounter - previousCounter) / Settings.StatusMessageCycleSeconds} r/s - Epochs: {EpochManager.EpochsLoaded} - Redirects: {RedirectMap.Count()} - Dead dogs: {DeadDogs.Count()}");
				previousCounter = rowCounter;
				EpochManager.VacuumPeriodic();
			};
			timer.AutoReset = true;
			timer.Enabled = true;

			Parallel.ForEach(Analysis.AsParallel().AsOrdered(), new ParallelOptions
			{
				MaxDegreeOfParallelism = Settings.MaxParallellism
			},
			analysisResult =>
			{
				string msg;
				var sourceFile = analysisResult.Path;

				try
				{
					msg = ProcessFile(sourceFile, ref rowCounter, ref fileCounter);
				}
				catch (Exception e1)
				{
					Log.Info("Encountered error: " + e1);
					Log.Info("Retrying..");
					try
					{
						msg = ProcessFile(sourceFile, ref rowCounter, ref fileCounter);
					}
					catch(Exception e2)
					{
						Log.Error("Encountered another error: " + e2);
						Log.Info("Giving up on " + Path.GetFileName(analysisResult.Path));
					}
				}
			});

			Log.Info("Dumping redirect map..");
			File.WriteAllText("RedirectMap.csv", DumpRedirectMap());

			Log.Info("Done!");
		}

		#endregion Public methods

		#region Utilities

		private string ProcessFile(string sourceFile, ref long globalRowCounter, ref int fileCounter)
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
			using (var writer = CreateStreamWriter(sourceFile))
			{
				WriteNewHeader(sourceFile, writer);

				long localRowCounter = 0;
				long currentRoundedEpoch = 0;
				while (csv.Read())
				{
					var timestamp = csv.GetDateTime(Settings.TimestampColumnName);
					var epoch = timestamp.ToEpoch();
					var roundedEpoch = Util.RoundToHourEpoch(timestamp);

					if (currentRoundedEpoch != roundedEpoch)
					{
						currentRoundedEpoch = roundedEpoch;
					}

					if (currentRoundedEpoch > MaximumEpoch)
					{
						// We don't have data for this epoch, skip it silently
						continue;
					}
					else if (currentRoundedEpoch < MinimumEpoch)
					{
						// We don't have data for this epoch, skip it silently
						continue;
					}


					if (!Settings.UseDirectSQL && !LoadEpoch(currentRoundedEpoch))
					{
						Log.Info($"Could not find epoch {currentRoundedEpoch} ({Util.DateTimeFromEpoch(currentRoundedEpoch)}) in database, skipping");
						continue;
					}

					var lat = csv.GetFloat(Settings.LatitudeColumnName);
					var lon = csv.GetFloat(Settings.LongitudeColumnName);
					var point = new Point(lon, lat);
					var hash = GeoHasher.Encode(point, Settings.GeohashMatchPrecision);

					WeatherHist match = null;

					if (!IsDeadDog(currentRoundedEpoch, hash))
					{
						var matches = EpochManager.Lookup(currentRoundedEpoch, Redirect(hash), false, Settings.UseDirectSQL);
						if (matches.Count() == 0)
						{
							DeepSearch(currentRoundedEpoch, hash, matches);
						}
						if (matches.Count() > 0)
						{
							match = SelectBestMatch(point, matches);
							MapRedirect(hash, GeoHasher.Reduce(match.Geohash, Settings.GeohashMatchPrecision));
						}
					}

					Interlocked.Increment(ref globalRowCounter);
					localRowCounter++;
					AppendWeatherData(csv, writer, match);
				}
				writer.Flush();
				writer.Close();

				if (localRowCounter == 0)
				{
					// Nothing was written, remove the empty file
					File.Delete(GetOutputFileName(sourceFile));
				}
			}

			Log.Info("Completed " + msg);
			return msg;
		}

		private WeatherHist SelectBestMatch(Point point, List<WeatherHist> matches)
		{
			WeatherHist match;
			if (!Settings.CalculateClosest)
			{
				match = matches.FirstOrDefault();
			}
			else if (Settings.CalculateClosest && matches.Count > 1)
			{
				match = matches.OrderBy(r => GeoHasher.Decode(r.Geohash).Distance(point)).First();
			}
			else
			{
				match = matches.FirstOrDefault();
			}

			return match;
		}

		private void AppendWeatherData(CsvDataReader csv, StreamWriter writer, WeatherHist result)
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

			writer.WriteLine(string.Join(Settings.Delimiter, rowData));
		}

		private bool LoadEpoch(long currentRoundedEpoch)
		{

			if (!EpochManager.IsLoaded(currentRoundedEpoch))
			{
				if (Settings.AssumeNoEpochCollissions)
				{
					return EpochManager.Load(currentRoundedEpoch);
				}
				else
				{
					lock (SyncRoot)
					{
						if (!EpochManager.IsLoaded(currentRoundedEpoch))
						{
							return EpochManager.Load(currentRoundedEpoch);
						}
						else
						{
							return true;
						}
					}
				}
			}
			else {
				return true;
			}
		}

		private void DeepSearch(long currentRoundedEpoch, string hash, List<WeatherHist> data)
		{
			var reducedHash = hash;
			while (true)
			{
				var fromNeighbours = FindNeighbours(currentRoundedEpoch, reducedHash, hash);
				var fromCurrentReducedHash = EpochManager.Lookup(currentRoundedEpoch,
													reducedHash,
														reducedHash.Length < Settings.GeohashMatchPrecision,
															Settings.UseDirectSQL, hash);
				data.AddRange(fromNeighbours);
				data.AddRange(fromCurrentReducedHash);

				if (data.Count() > 0)
				{
					break;
				}

				if (GeoHasher.Reduce(reducedHash).Length == Settings.PrecisionSearchLimit - 1)
				{
					KillTheDog(currentRoundedEpoch, hash);
					break;
				}

				reducedHash = GeoHasher.Reduce(reducedHash);
			}
		}

		private bool IsDeadDog(long currentRoundedEpoch, string hash)
		{
			var dog = currentRoundedEpoch.ToString() + "-" + hash;
			return DeadDogs.Contains(dog);
		}

		private void KillTheDog(long currentRoundedEpoch, string hash)
		{
			var dog = currentRoundedEpoch.ToString() + "-" + hash;
			DeadDogs.Add(dog);
		}

		private void WriteNewHeader(string sourceFile, StreamWriter writer)
		{
			var header = File.ReadLines(sourceFile).First().Split(";").ToList();
			header.Add("hs");
			header.Add("thq");
			header.Add("ff");
			header.Add("dd");
			writer.WriteLine(string.Join(Settings.Delimiter, header.ToArray()));
		}

		private StreamWriter CreateStreamWriter(string sourceFile)
		{
			var outputFileName = GetOutputFileName(sourceFile);
			var writer = new StreamWriter(outputFileName, false, Encoding.UTF8, 65536); // 4MB buffer
			writer.AutoFlush = true;
			return writer;
		}

		private string GetOutputFileName(string sourceFile)
		{
			return Path.Combine(Settings.AisDestinationDirectory, Path.GetFileName(sourceFile) + ".weather.csv");
		}

		private List<WeatherHist> FindNeighbours(long currentRoundedEpoch, string reducedHash, string originalHash)
		{
			var neighbours = GeoHasher.Neighbours(reducedHash);
			bool expandedSearch = false;
			int previousCount = neighbours.Count();

			var data = new List<WeatherHist>();
			foreach (var n in neighbours)
			{
				var tmp = EpochManager.Lookup(currentRoundedEpoch, n, n.Length < Settings.GeohashMatchPrecision, Settings.UseDirectSQL, originalHash);
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