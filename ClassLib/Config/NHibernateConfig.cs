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
            .Mappings(m => m
                .FluentMappings.AddFromAssemblyOf<EmployeeMap>()
            )
            .ExposeConfiguration(cfg =>
            {
                var schemaUpdate = new SchemaUpdate(cfg);
                schemaUpdate.Execute(false, true);

                cfg.SetProperty("hibernate.dialect", "NHibernate.Dialect.MySQL8Dialect");
                cfg.SetProperty("hibernate.connection.driver_class", "NHibernate.Driver.MySqlDataDriver");
                cfg.SetProperty("hibernate.id.new_generator_mappings", "true");
            })
            .BuildSessionFactory();

        return config;
    }
}