using FluentNHibernate.Conventions;
using GravityVectorToolKit.Common;
using GravityVectorToolKit.Tools.AisCombine.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GravityVectorToolKit.Tools.AisCombine
{
	public class EpochManager
	{
		private static ILog Log = LogManager.GetLogger(typeof(AisCombiner));

		int maxCapacity = Environment.ProcessorCount*256;

		Dictionary<long, Dictionary<string, List<WeatherHist>>> Index 
			= new Dictionary<long, Dictionary<string, List<WeatherHist>>>();

		Dictionary<long, DateTime> EpochLastAccessed = new Dictionary<long, DateTime>();
		private object syncRoot = new object();

		public string DatabasePath { get; }

		public EpochManager(string db)
		{
			DatabasePath = db;
		}

		public bool IsAvailable(long currentRoundedEpoch)
		{
			return Index.ContainsKey(currentRoundedEpoch);
		}

		public void Load(long epoch)
		{
			if (!Index.ContainsKey(epoch))
			{
				Vacuum(); // Free up space if possible first

				Log.Info("Loading epoch " + epoch);
				var context = new MadartWeatherDbContext(DatabasePath);
				var result = context.WeatherHists.Where(w => w.Epoch == epoch).ToList();

				foreach(var weatherHist in result)
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
			var toBeRemoved = EpochLastAccessed.ToList().Where(e => (DateTime.Now - e.Value).TotalSeconds > 15 * 60).ToList();
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

		public List<WeatherHist> Lookup(long epoch, string hash, bool reducedPrecision)
		{
			if (Index.ContainsKey(epoch))
			{
				EpochLastAccessed[epoch] = DateTime.Now;

				if (reducedPrecision)
				{
					return Index[epoch].Where(e => e.Key.StartsWith(hash)).SelectMany(e => e.Value).ToList();
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
	}
}