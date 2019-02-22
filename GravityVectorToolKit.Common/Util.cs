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

		public static ITransaction BeginTransaction(ISession session)
		{
			//return TryNTimes(() =>
			//{
				return session.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
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