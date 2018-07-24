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
			var csv = new CsvReader(File.OpenText(file), new Configuration
			{
				CultureInfo = CultureInfo.InvariantCulture,
				HeaderValidated = null,
				MissingFieldFound = null			
			});
			csv.Configuration.RegisterClassMap<ModelClassMapG>();

			var records = csv.GetRecords<NormalPointG>().ToList();
			return records;
		}
	}
}
