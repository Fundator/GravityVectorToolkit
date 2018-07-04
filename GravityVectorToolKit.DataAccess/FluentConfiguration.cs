using FluentNHibernate.Cfg;
using GravityVectorToKML.CSV.Mapping;
using GravityVectorToKML.Model;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
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
                .Database(FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard
                .ConnectionString(c => c.Server("localhost").Database("gravityvectortoolkit").Username("gvtk").Password("gvtk"))
                .Driver<MySqlDataDriver>()
                .Dialect<MySQL57SpatialDialect>())
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<NormalPointGMapping>())
                .BuildConfiguration();

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
