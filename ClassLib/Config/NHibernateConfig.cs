using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace ClassLib.Config;

public class NHibernateConfig
{
    private readonly string _connectionString;

    public NHibernateConfig(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public ISessionFactory CreateSessionFactory()
    {
        var config = Fluently.Configure()
            .Database(MySQLConfiguration.Standard
                .ConnectionString(_connectionString)
                .ShowSql() 
                .FormatSql()
            )
            .Mappings(m =>
                m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
            )
            .ExposeConfiguration(cfg =>
            {
                // в демонстрационных целях!!!
                var schemaExport = new SchemaExport(cfg);
                schemaExport.Drop(false, true); 
                schemaExport.Create(false, true);
                // а так
                //var schemaUpdate = new SchemaUpdate(cfg);
                //schemaUpdate.Execute(false, true);

                cfg.SetProperty("hibernate.dialect", "NHibernate.Dialect.MySQL5Dialect");
                cfg.SetProperty("hibernate.connection.driver_class", "NHibernate.Driver.MySqlDataDriver");
                cfg.SetProperty("hibernate.id.new_generator_mappings", "true");
            })
            .BuildSessionFactory();

        return config;
    }
}