using System;
using System.Collections.Generic;
using System.IO;

namespace GravityVectorToolKit.Tools.AisCombine
{
	public class AisCombinerSettings
	{
		public string AisSourceDirectory;
		public string AisDestinationDirectory;
		public string WeatherDbPath;
		public int MaxParallellism = Environment.ProcessorCount;
		public bool AssumeNoEpochCollissions = false;
		public string TimestampColumnName = "date_time_utc";
		public string LatitudeColumnName = "lat";
		public string LongitudeColumnName = "lon";
		public bool CalculateClosest = true;
		public int GeohashMatchPrecision = 5;
		public int PrecisionSearchLimit = 3;
		public char Delimiter = ';';
		public int StatusMessageCycleSeconds = 10;
		public int MaximumEpochAgeMinutes = 15;
		public string SortBy = "path";
		public bool UseDirectSQL = false;

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