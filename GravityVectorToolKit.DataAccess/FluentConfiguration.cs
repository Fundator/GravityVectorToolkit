using FluentNHibernate.Cfg;
using GravityVectorToolKit.CSV.Mapping;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Type;
using NHibernate.Tool.hbm2ddl;

namespace GravityVectorToolKit.DataAccess
{
	public static class FluentConfiguration
	{
		public static void Configure(string connectionString, bool dropAndCreateTables = false)
		{
			var cfg = Fluently.Configure()
				.Database(FluentNHibernate.Cfg.Db.PostgreSQLConfiguration.Standard
				.ConnectionString(connectionString)
				.Driver</*NHibernate.Extensions.NpgSql.NpgSqlDriver*/NpgsqlDriver>()
				.Dialect<PostGis20Dialect>())
				.Mappings(x =>
					{
						x.FluentMappings.Add(typeof(GravityVectorMapping<PostGisGeometryType>));
						x.FluentMappings.Add(typeof(NormalRouteMapping<PostGisGeometryType>));
						x.FluentMappings.Add(typeof(DeviationCellMapping<PostGisGeometryType>));
						x.FluentMappings.Add(typeof(NearMissIncidentMapping<PostGisGeometryType>));
					}
				)
				.BuildConfiguration()
				.SetProperty("command_timeout", "-1");

			cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));

			if (dropAndCreateTables)
			{
				var exporter = new SchemaExport(cfg);
				exporter.Drop(false, true);
				exporter.Create(false, true);
			}

			SessionManager.SessionFactory = cfg.BuildSessionFactory();
		}

		public static void TestConfiguration(string connectionString)
		{
			Configure(connectionString, false);
		}
	}
}