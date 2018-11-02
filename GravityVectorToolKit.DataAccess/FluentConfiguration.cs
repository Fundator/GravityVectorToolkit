using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;
using GravityVectorToolKit.CSV.Mapping;
using GravityVectorToolKit.DataModel;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Type;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataAccess
{
	public static class FluentConfiguration
	{
		public static void Configure(bool generateTables = false)
		{

			var cfg = Fluently.Configure()
				//.Database(FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard
				//.ConnectionString("Server=localhost;Database=gravityvectortoolkit;Connect Timeout=36000;port=3306;Uid=gvtk;Pwd=gvtk")
				//.Driver<MySqlDataDriver>()
				//.Dialect<MySQL57SpatialDialect>())
				.Database(FluentNHibernate.Cfg.Db.PostgreSQLConfiguration.Standard
				.ConnectionString("Server=localhost;Port=5432;Database=gvtk;User Id=gvtk;Password = gvtk;")
				.Driver<NpgsqlDriver>()
				.Dialect<PostGis20Dialect>())
				.Mappings(x => x.FluentMappings.Add(typeof(NormalPointGMapping<PostGisGeometryType>)))
				.BuildConfiguration()
				.SetProperty("command_timeout", "-1");

			cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));

			if (generateTables)
			{
				var exporter = new SchemaExport(cfg);
				exporter.Drop(false, true);
				exporter.Create(true, true);
			}

			SessionManager.SessionFactory = cfg.BuildSessionFactory();

		}
	}


	public static class SessionManager
	{
		private static ISession _session;
		public static ISessionFactory SessionFactory;
		public static ISession Session
		{
			get
			{
				if (_session == null)
				{
					if (SessionFactory == null)
					{
						FluentConfiguration.Configure();
					}
					_session = SessionFactory.OpenSession();
				}
				return _session;
			}
		}
	}
}
