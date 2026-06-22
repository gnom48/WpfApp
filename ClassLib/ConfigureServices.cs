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
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<NHibernateConfig>();

        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        services.AddScoped<IOrderService, OrderService>();

        services.AddScoped<ISession>(provider =>
        {
            return NHibernateHelper.OpenSession();
        });

        return services;
    }
}