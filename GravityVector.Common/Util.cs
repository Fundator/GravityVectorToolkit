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
		public static List<NormalPoint> ReadGravityVector(string file)
		{
			StreamReader reader = File.OpenText(file);
			var csv = new CsvReader(reader, new Configuration
			{
				CultureInfo = CultureInfo.InvariantCulture,
				HeaderValidated = null,
				MissingFieldFound = null,
				Delimiter = ","
			});
			csv.Configuration.RegisterClassMap<ModelClassMapG>();

			var records = csv.GetRecords<NormalPoint>().ToList();
			reader.Close();
			reader.Dispose();
			csv.Dispose();
			return records;
		}
	}
}
