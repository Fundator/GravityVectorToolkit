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
			//var t = dt - new DateTime(1970, 1, 1);
			//int secondsSinceEpoch = (int)t.TotalSeconds;

			var t = (new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0))
				.Subtract(new DateTime(1970, 1, 1));

			return (int)t.TotalSeconds;
			//dt
			//long ticks = dt.Ticks + 18000000000;
			//ticks = ticks - ticks % 36000000000;
			//long epoch = ticks / 1000;
			//return epoch;
		}

		public static long ToEpoch(this DateTime date)
		{
			var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			var diff = date.ToUniversalTime() - origin;
			return (long)Math.Floor(diff.TotalSeconds);
		}

		/// <summary>
		/// Execute the given function n times and catch/log any exceptions
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="f"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		//public static T TryNTimes<T>(Func<T> f, int n)
		//{
		//	int i = 0;
		//	while (true)
		//	{
		//		try
		//		{
		//			return f();
		//		}
		//		catch (Exception)
		//		{
		//			if (i == n)
		//			{
		//				throw;
		//			}
		//			else
		//			{
		//				Log.Warn($"Caught exception, retrying {i}/{n} times..");
		//				i++;
		//			}
		//		}
		//	}
		//}
	}
}