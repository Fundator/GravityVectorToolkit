using CsvHelper;
using CsvHelper.Configuration;
using GravityVectorToolKit.DataAccess;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GravityVectorToolKit.Common
{
	public static class Util
	{
		private static CsvConfiguration DefaultConfig => new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HeaderValidated = null,
			MissingFieldFound = null,
			Delimiter = ",",
			BufferSize = 1048576,
		};

		public static List<TModel> ReadCsvFile<TModel, TClassMap>(string file) where TClassMap : ClassMap<TModel>
		{
			StreamReader reader = File.OpenText(file);
			var csv = new CsvReader(reader, DefaultConfig);
			csv.Context.RegisterClassMap<TClassMap>();
			var records = csv.GetRecords<TModel>().ToList();
			reader.Close();
			reader.Dispose();
			csv.Dispose();
			return records;
		}

		public static IEnumerable<TModel> ReadCsvFileByRow<TModel, TClassMap>(string file) where TClassMap : ClassMap<TModel>
		{
			StreamReader reader = File.OpenText(file);
			var csv = new CsvReader(reader, DefaultConfig);
			csv.Context.RegisterClassMap<TClassMap>();
			//var records = csv.GetRecords<TModel>().ToList();
			while (csv.Read())
			{
				var record = csv.GetRecord<TModel>();
				yield return record;
			}
			reader.Close();
			reader.Dispose();
			csv.Dispose();
		}

		public static IEnumerable<TModel> ReadFromList<TModel, TClassMap>(IEnumerable<string> lines) where TClassMap : ClassMap<TModel>
		{
			var reader = new StringReader(string.Join("\r\n", lines.ToArray()));
			var csv = new CsvReader(reader, DefaultConfig);
			csv.Context.RegisterClassMap<TClassMap>();
			var records = csv.GetRecords<TModel>().ToList();
			reader.Close();
			reader.Dispose();
			return records;
		}

		public static ITransaction BeginTransaction(ISession session)
		{
			//return TryNTimes(() =>
			//{
			return session.BeginTransaction(System.Data.IsolationLevel.Snapshot);
			//}, 10);
		}

		public static ISession GetSession()
		{
			//return TryNTimes(() =>
			//{
			var session = SessionManager.SessionFactory.OpenSession();
			session.FlushMode = FlushMode.Manual;
			return session;
			//}, 10);
		}

		public static long RoundToHourEpoch(DateTime dt)
		{

			var t = (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0))
				.Subtract(new DateTime(1970, 1, 1));
			return (int)t.TotalSeconds;
		}

		static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long ToEpoch(this DateTime date)
		{
			var diff = date.ToUniversalTime() - epoch;
			return (long)Math.Floor(diff.TotalSeconds);
		}

		public static DateTime DateTimeFromEpoch(long timestamp)
		{
			return epoch.AddSeconds(timestamp);
		}

		public static string FormatFileSize(long bytes)
		{
			var unit = 1024;
			if (bytes < unit) { return $"{bytes} B"; }

			var exp = (int)(Math.Log(bytes) / Math.Log(unit));
			return $"{bytes / Math.Pow(unit, exp):F2} {("KMGTPE")[exp - 1]}B";
		}
	}
}