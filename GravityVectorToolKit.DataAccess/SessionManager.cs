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
					_session = SessionFactory.OpenSession();
				}
				return _session;
			}
		}
	}
}