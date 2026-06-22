using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace ClassLib.Config;

public static class NHibernateHelper
{
    private static ISessionFactory _sessionFactory;
    private static readonly object _lockObj = new object();

    public static void Initialize(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public static ISession OpenSession()
    {
        if (_sessionFactory == null)
            throw new InvalidOperationException("NHibernate not initialized. Call Initialize first.");

        return _sessionFactory.OpenSession();
    }
}