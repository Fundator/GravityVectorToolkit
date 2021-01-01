using NHibernate;

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