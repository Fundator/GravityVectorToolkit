using FluentNHibernate.Conventions;
using GravityVectorToolKit.Common;
using GravityVectorToolKit.Tools.AisCombine.DataAccess;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GravityVectorToolKit.Tools.AisCombine
{
	public class EpochManager
	{
		ThreadLocal<MadartWeatherDbContext> ThreadLocalDbContext;
		ThreadLocal<Dictionary<long, SortedDictionary<string, WeatherHist>>> ThreadLocalQueryCache;
		ConcurrentDictionary<long, ConcurrentDictionary<string, WeatherHist>> QueryCache;

		private readonly int MaximumEpochAgeMinutes;
		private static ILog Log = LogManager.GetLogger(typeof(AisCombiner));

		int maxCapacity = Environment.ProcessorCount*256;

		Dictionary<long, Dictionary<string, List<WeatherHist>>> Index 
			= new Dictionary<long, Dictionary<string, List<WeatherHist>>>();

		Dictionary<long, DateTime> EpochLastAccessed = new Dictionary<long, DateTime>();
		private object syncRoot = new object();

		public string DatabasePath { get; }

		public int CacheMiss { get => cacheMiss; }
		public int CacheHit { get => cacheHit; }
		public int Lookups { get => cacheHit+cacheMiss; }

		public double CacheHitRatePct { 
			get 
			{
				return Math.Round(((cacheHit*1.0d) / (Lookups *1.0d)) * 100.0d, 2);
			} 
		}

		public EpochManager(string db, int maximumEpochAgeMinutes = 15)
		{
			DatabasePath = db;
			MaximumEpochAgeMinutes = maximumEpochAgeMinutes;
			ThreadLocalDbContext = new ThreadLocal<MadartWeatherDbContext>(() =>
			{
				var context = new MadartWeatherDbContext(DatabasePath);
				context.Database.ExecuteSqlRaw("PRAGMA read_uncommitted = true;");
				context.Database.ExecuteSqlRaw("PRAGMA cache_size = 400000;");
				context.Database.ExecuteSqlRaw("PRAGMA mmap_size = 30000000000;");
				return context;
			});

			ThreadLocalQueryCache = new ThreadLocal<Dictionary<long, SortedDictionary<string, WeatherHist>>>(() =>
			{
				return new Dictionary<long, SortedDictionary<string, WeatherHist>>();
			});

			QueryCache = new ConcurrentDictionary<long, ConcurrentDictionary<string, WeatherHist>>();

		}

		public int EpochsLoaded
		{
			get
			{
				return Index.Count();
			}
		}



		public bool IsLoaded(long currentRoundedEpoch)
		{
			return Index.ContainsKey(currentRoundedEpoch);
		}

		public bool Load(long epoch)
		{
			if (!Index.ContainsKey(epoch))
			{
				Vacuum(); // Free up space if possible first

				var context = new MadartWeatherDbContext(DatabasePath);
				var result = context.WeatherHists.Where(w => w.Epoch == epoch).ToList();

				if (result.Any())
				{
					foreach (var weatherHist in result)
					{
						var hash = weatherHist.Geohash.Substring(0, 5);
						if (!Index.ContainsKey(epoch))
						{
							lock (syncRoot)
							{
								Index[epoch] = new Dictionary<string, List<WeatherHist>>();
							}
						}

						if (!Index[epoch].ContainsKey(hash))
						{
							lock (syncRoot)
							{
								Index[epoch][hash] = new List<WeatherHist>();
							}
						}

						Index[epoch][hash].Add(weatherHist);
					}

					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return true;
			}
		}

		private void Vacuum()
		{
			if (Index.Count >= (maxCapacity * 1.10))
			{
				Log.Info("Vacuuming..");

				// Remove the oldest entries above capacity
				var toBeRemoved = EpochLastAccessed.OrderBy(kvp => kvp.Value).Take(Index.Count-maxCapacity).ToList();
				var youngestEntry = (DateTime.Now - toBeRemoved.Last().Value).TotalSeconds;
				if (youngestEntry < 300)
				{
					Log.Info($"The epoch cache seems to be vacuuming non-stale entries, consider increasing the capacity (current capacity is {maxCapacity}, and the youngest entry I just vacuumed was {youngestEntry} seconds old)");
				}
				foreach (var k in toBeRemoved)
				{
					Index.Remove(k.Key);
					EpochLastAccessed.Remove(k.Key);
					Log.Info($"Epoch {k.Key} was vacuumed");
				}

				GC.Collect();
			}
		}
		public void VacuumPeriodic()
		{
			var toBeRemoved = EpochLastAccessed.ToList().Where(e => (DateTime.Now - e.Value).TotalSeconds > (MaximumEpochAgeMinutes * 60)).ToList();
			if (toBeRemoved.Any())
			{
				Log.Info("Vacuuming old epoch entries..");
				lock (syncRoot)
				{
					foreach (var k in toBeRemoved)
					{
						Index.Remove(k.Key);
						EpochLastAccessed.Remove(k.Key);
						var diff = (DateTime.Now - k.Value);
						Log.Info($"Epoch {k.Key} was vacuumed due to old age ({diff.TotalMinutes}m{diff.Seconds}s)");
					}
					GC.Collect();
				}
			}
		}

		GvtkGeohasher Hasher = new GvtkGeohasher();
		int cacheMiss = 0;
		int cacheHit = 0;
		public List<WeatherHist> Lookup(long epoch, string hash, bool reducedPrecision, bool useSql = false, string originalHash = "")
		{

			//Interlocked.Increment(ref lookups);

			if (originalHash == string.Empty)
			{
				originalHash = hash;
			}
			if (!useSql)
			{
				if (Index.ContainsKey(epoch))
				{
					EpochLastAccessed[epoch] = DateTime.Now;

					if (reducedPrecision)
					{
						return Index[epoch].ToList().Where(e => e.Key.StartsWith(hash)).SelectMany(e => e.Value).ToList();
					}
					else if (Index[epoch].ContainsKey(hash))
					{
						return Index[epoch][hash];
					}
					else
					{
						return new List<WeatherHist>();
					}
				}
				else
				{
					throw new Exception($"This epoch ({epoch}) is not loaded");
				}
			}
			else
			{

				// Check the cache first
				var queryCache = QueryCache;
				if (queryCache.TryGetValue(epoch, out var records))
				{


					var tmp = records.Where(kvp => kvp.Key.StartsWith(hash)).Select(kvp => kvp.Value).ToList();
					if (tmp.Count() > 0)
					{
						Interlocked.Increment(ref cacheHit);
						return tmp;
					}
				}

				// Query the database
				var nudgedHashHigh = StringNudge(hash, 1);
				var nudgedHashLow = StringNudge(hash, -1);
				var context = ThreadLocalDbContext.Value;
				//var result = context.WeatherHists.Where(wh => (wh.Epoch == epoch) && wh.Geohash.StartsWith(Hasher.Reduce(hash))).ToList();
				var result = context.WeatherHists.Where(wh => (wh.Epoch == epoch) && wh.Geohash.CompareTo(nudgedHashLow) > 0 && wh.Geohash.CompareTo(nudgedHashHigh) < 0).ToList();

				foreach (var w in result)
				{
					if (!queryCache.ContainsKey(w.Epoch))
					{
						queryCache[w.Epoch] = new ConcurrentDictionary<string, WeatherHist>();
					}
					queryCache[w.Epoch].TryAdd(w.Geohash, w);
				}

				Interlocked.Increment(ref cacheMiss);
				return result;
			}
		}

		private string StringNudge(string hash, int v)
		{
			var chars = hash.ToCharArray();
			chars[chars.Length - 1] = (char)(Convert.ToUInt16(chars[chars.Length - 1]) + v);
			return new string(chars);
		}
	}
}