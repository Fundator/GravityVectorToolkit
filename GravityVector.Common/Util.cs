using CsvHelper;
using CsvHelper.Configuration;
using GravityVectorToolKit.CSV.Mapping;
using GravityVectorToolKit.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GravityVector.Common
{
	public static class Util
	{
		static Configuration DefaultConfig = new Configuration
		{
			CultureInfo = CultureInfo.InvariantCulture,
			HeaderValidated = null,
			MissingFieldFound = null,
			Delimiter = ",",
			BufferSize = 1048576
		};

		public static List<NormalPoint> ReadGravityVector(string file)
		{
			StreamReader reader = File.OpenText(file);
			var csv = new CsvReader(reader, DefaultConfig);
			csv.Configuration.RegisterClassMap<NormalPointCsvClassMap>();

			var records = csv.GetRecords<NormalPoint>().ToList();
			reader.Close();
			reader.Dispose();
			csv.Dispose();
			return records;
		}

		public static List<NormalRoute> ReadNormalRouteFile(string file)
		{
			StreamReader reader = File.OpenText(file);
			var csv = new CsvReader(reader, DefaultConfig);
			csv.Configuration.RegisterClassMap<NormalRouteCsvClassMap>();

			var records = csv.GetRecords<NormalRoute>().ToList();
			reader.Close();
			reader.Dispose();
			csv.Dispose();
			return records;
		}
	}
}
