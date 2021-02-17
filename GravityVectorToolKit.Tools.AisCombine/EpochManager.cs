using FluentNHibernate.Conventions;
using GravityVectorToolKit.Common;
using GravityVectorToolKit.Tools.AisCombine.DataAccess;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

		public EpochManager(string db, int maximumEpochAgeMinutes = 15)
		{
			DatabasePath = db;
			MaximumEpochAgeMinutes = maximumEpochAgeMinutes;
			ThreadLocalDbContext = new ThreadLocal<MadartWeatherDbContext>(() =>
			{
				return new MadartWeatherDbContext(DatabasePath);
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
				if (queryCache.ContainsKey(epoch))
				{
					var tmp = queryCache[epoch].ToList().Where(kvp => kvp.Key.StartsWith(hash)).Select(kvp => kvp.Value).ToList();
					if (tmp.Count() > 0)
					{
						Interlocked.Increment(ref cacheHit);
						return tmp;
					}
				}

				// Query the database
				var context = ThreadLocalDbContext.Value;
				var result = context.WeatherHists.Where(wh => (wh.Epoch == epoch) && wh.Geohash.StartsWith(hash)).ToList();

				foreach (var w in result)
				{
					if (!queryCache.ContainsKey(w.Epoch))
					{
						queryCache[w.Epoch] = new ConcurrentDictionary<string, WeatherHist>();
					}
					if (!queryCache[w.Epoch].TryAdd(w.Geohash, w))
					{
						//Log.Warn($"Unable to add {w.Epoch}/{w.Geohash} to cache");
					}
				}

				Interlocked.Increment(ref cacheMiss);
				return result;

			}
		}
	}
}