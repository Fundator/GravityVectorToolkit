using CsvHelper;
using CsvHelper.Configuration;
using GravityVectorToKML.CSV.Mapping;
using GravityVectorToKML.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GravityVector.Common
{
	public class Util
	{
		public static List<NormalPointG> ReadGravityVector(string file)
		{
			StreamReader reader = File.OpenText(file);
			var csv = new CsvReader(reader, new Configuration
			{
				CultureInfo = CultureInfo.InvariantCulture,
				HeaderValidated = null,
				MissingFieldFound = null			
			});
			csv.Configuration.RegisterClassMap<ModelClassMapG>();

			var records = csv.GetRecords<NormalPointG>().ToList();
			reader.Close();
			reader.Dispose();
			csv.Dispose();
			return records;
		}
	}
}
