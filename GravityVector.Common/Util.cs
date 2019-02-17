using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GravityVector.Common
{
	public static class Util
	{
		private static Configuration DefaultConfig => new Configuration
		{
			CultureInfo = CultureInfo.InvariantCulture,
			HeaderValidated = null,
			MissingFieldFound = null,
			Delimiter = ",",
			BufferSize = 1048576
		};

		public static List<TModel> ReadCsvFile<TModel, TClassMap>(string file) where TClassMap : ClassMap<TModel>
		{
			StreamReader reader = File.OpenText(file);
			var csv = new CsvReader(reader, DefaultConfig);
			csv.Configuration.RegisterClassMap<TClassMap>();
			var records = csv.GetRecords<TModel>().ToList();
			reader.Close();
			reader.Dispose();
			csv.Dispose();
			return records;
		}
	}
}