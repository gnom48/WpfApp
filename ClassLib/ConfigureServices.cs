using ClassLib.Config;
using ClassLib.Repositories.Interfaces;
using ClassLib.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using WpfApp.Data.Services.interfaces;
using WpfApp.Data.Services;

namespace WpfApp.Data;

public static class ConfigureServices
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton<NHibernateConfig>();

        services.AddSingleton<ISessionFactory>(provider =>
        {
            var config = provider.GetRequiredService<NHibernateConfig>();
            return config.CreateSessionFactory();
        });

        services.AddScoped<ISession>(provider =>
        {
            var factory = provider.GetRequiredService<ISessionFactory>();
            return factory.OpenSession();
        });

        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}