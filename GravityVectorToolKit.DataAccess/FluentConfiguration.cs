using FluentNHibernate.Cfg;
using GravityVectorToolKit.CSV.Mapping;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Type;
using NHibernate.Tool.hbm2ddl;

namespace GravityVectorToolKit.DataAccess
{
	public static class FluentConfiguration
	{
		public static void Configure(string connectionString, bool generateTables = false)
		{
			var cfg = Fluently.Configure()
				.Database(FluentNHibernate.Cfg.Db.PostgreSQLConfiguration.Standard
				.ConnectionString(connectionString)
				.Driver<NpgsqlDriver>()
				.Dialect<PostGis20Dialect>())
				.Mappings(x =>
					{
						x.FluentMappings.Add(typeof(NormalPointMapping<PostGisGeometryType>));
						x.FluentMappings.Add(typeof(NormalRouteMapping<PostGisGeometryType>));
					}
				)
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
}