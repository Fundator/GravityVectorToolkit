using System;
using System.Collections.Generic;
using System.IO;

namespace GravityVectorToolKit.Tools.AisCombine
{
	public class AisCombinerSettings
	{
		public readonly string Id = Guid.NewGuid().ToString();
		public string AisSourceDirectory;
		public string AisDestinationDirectory;
		public string WeatherDbPath;
		public int MaxParallellism = Environment.ProcessorCount;
		public bool AssumeNoEpochCollissions = false;

		public readonly string TimestampColumnName = "date_time_utc";
		public readonly string LatitudeColumnName = "lat";
		public readonly string LongitudeColumnName = "lon";

		public int CalculateClosestThreshold = 7;
		public bool CalculateClosest = true;
		internal int Precision = 5;

		public int PrecisionSearchLimit = 3;

		public List<string> Validate()
		{
			var result = new List<string>();
			if (!Directory.Exists(AisSourceDirectory))
			{
				result.Add($"The AIS source path {AisSourceDirectory} does not exist");
			}
			if (!Directory.Exists(AisDestinationDirectory))
			{
				result.Add($"The AIS destination path {AisDestinationDirectory} does not exist");
			}
			if (!File.Exists(WeatherDbPath))
			{
				result.Add($"The weather database file {WeatherDbPath} does not exist");
			}

			return result;
		}
	}
}