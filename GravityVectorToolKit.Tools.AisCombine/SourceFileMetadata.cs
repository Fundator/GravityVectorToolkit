namespace GravityVectorToolKit.Tools.AisCombine
{
	public class SourceFileMetadata
	{
		public SourceFileMetadata()
		{
		}
		public string Path { get; internal set; }
		public long LineCount { get; internal set; }
	}
}

#region old

//public static IEnumerable<string[]> ReadAisCsv(string filename, MadartWeatherDbContext context)
//{
//	Log.Info($"Combining {filename}..");
//	int count = 0;
//	var hasher = new GvtkGeohasher();

//	using (var stream = File.OpenRead(filename))
//	using (var reader = new StreamReader(stream))
//	using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
//	{
//		Delimiter = ";",
//		HasHeaderRecord = true
//	}))
//	{
//		stream.Position = 0;
//		csv.Read();
//		csv.ReadHeader();

//		var resultHeaders = new List<string>(csv.HeaderRecord);
//		resultHeaders.Add("hs");
//		resultHeaders.Add("thq");
//		resultHeaders.Add("ff");
//		resultHeaders.Add("dd");
//		yield return resultHeaders.ToArray();

//		while (csv.Read())
//		{
//			count++;
//			float lat = 0, lon = 0;
//			var timestamp = DateTime.MinValue;
//			var headers = csv.HeaderRecord;
//			var rowValues = new List<string>();
//			foreach (var header in headers)
//			{
//				rowValues.Add(csv.GetField<string>(header));
//				if (header == "lat")
//				{
//					lat = csv.GetField<float>("lat");
//				}
//				else if (header == "lon")
//				{
//					lon = csv.GetField<float>("lon");
//				}
//				else if (header == "date_time_utc")
//				{
//					timestamp = csv.GetField<DateTime>("date_time_utc");
//				}
//			}
//			var point = new Point(lat, lon);
//			var hash = hasher.Encode(point, 5);
//			var result = context.WeatherHists
//				.Where(w => w.Geohash.StartsWith(hash))
//				.Where(w => w.Epoch == Util.RoundToHourEpoch(timestamp)).ToList();

//			if (!result.Any())
//			{
//				// Expand search
//				var neighbours = hasher.Neighbours(hash);
//				foreach (var n in neighbours)
//				{
//					result.AddRange(context.WeatherHists
//						.Where(w => w.Geohash.StartsWith(n))
//						.Where(w => w.Epoch == Util.RoundToHourEpoch(timestamp)).ToList());
//					if (result.Any())
//					{
//						break;
//					}
//				}
//			}

//			//var closest = result.OrderBy(r => hasher.Decode(r.Geohash))

//			var bestMatch = result.FirstOrDefault();
//			if (bestMatch != null)
//			{
//				rowValues.Add(bestMatch.Hs.ToString());
//				rowValues.Add(bestMatch.Thq.ToString());
//				rowValues.Add(bestMatch.Ff.ToString());
//				rowValues.Add(bestMatch.Dd.ToString());
//			}

//			if (count % 10 == 0)
//			{
//				Log.Info($"Completed {count} rows");
//			}

//			yield return rowValues.ToArray();
//		}
//	}
//}

//public static lol(string filename)
//{
//	using (var stream = File.OpenRead(filename))
//	using (var reader = new StreamReader(stream))
//	using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
//	{
//		Delimiter = ";",
//		HasHeaderRecord = true
//	}))
//	{
//		stream.Position = 0;
//		csv.Read();
//		csv.ReadHeader();

//		var resultHeaders = new List<string>(csv.HeaderRecord);
//		resultHeaders.Add("hs");
//		resultHeaders.Add("thq");
//		resultHeaders.Add("ff");
//		resultHeaders.Add("dd");
//		yield return resultHeaders.ToArray();

//		while (csv.Read())
//		{
//			count++;
//			float lat = 0, lon = 0;
//			var timestamp = DateTime.MinValue;
//			var headers = csv.HeaderRecord;
//			var rowValues = new List<string>();
//			foreach (var header in headers)
//			{
//				rowValues.Add(csv.GetField<string>(header));
//				if (header == "lat")
//				{
//					lat = csv.GetField<float>("lat");
//				}
//				else if (header == "lon")
//				{
//					lon = csv.GetField<float>("lon");
//				}
//				else if (header == "date_time_utc")
//				{
//					timestamp = csv.GetField<DateTime>("date_time_utc");
//				}
//			}
//			var point = new Point(lat, lon);
//			var hash = hasher.Encode(point, 5);
//			var result = context.WeatherHists
//				.Where(w => w.Geohash.StartsWith(hash))
//				.Where(w => w.Epoch == Util.RoundToHourEpoch(timestamp)).ToList();

//			if (!result.Any())
//			{
//				// Expand search
//				var neighbours = hasher.Neighbours(hash);
//				foreach (var n in neighbours)
//				{
//					result.AddRange(context.WeatherHists
//						.Where(w => w.Geohash.StartsWith(n))
//						.Where(w => w.Epoch == Util.RoundToHourEpoch(timestamp)).ToList());
//					if (result.Any())
//					{
//						break;
//					}
//				}
//			}

//			//var closest = result.OrderBy(r => hasher.Decode(r.Geohash))

//			var bestMatch = result.FirstOrDefault();
//			if (bestMatch != null)
//			{
//				rowValues.Add(bestMatch.Hs.ToString());
//				rowValues.Add(bestMatch.Thq.ToString());
//				rowValues.Add(bestMatch.Ff.ToString());
//				rowValues.Add(bestMatch.Dd.ToString());
//			}

//			if (count % 10 == 0)
//			{
//				Log.Info($"Completed {count} rows");
//			}

//			yield return rowValues.ToArray();
//		}
//	}
//}

#endregion old