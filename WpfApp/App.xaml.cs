using System;
using System.Configuration;
using System.Data;
using System.Windows;
using ClassLib.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using WpfApp.Data;

namespace WpfApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ServiceProvider ServiceProvider { get; private set; }
    public static IConfiguration _config { get; private set; }

    public App(IConfiguration config)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
        _config = config;
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddDataServices(_config);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            // Инициализация NHibernate
            using (var scope = ServiceProvider.CreateScope())
            {
                var sessionFactory = scope.ServiceProvider.GetRequiredService<ISessionFactory>();
                NHibernateHelper.Initialize(sessionFactory);

                // Проверка подключения к БД
                using (var session = sessionFactory.OpenSession())
                {
                    var version = session.CreateSQLQuery("SELECT VERSION()").UniqueResult<string>();
                    System.Diagnostics.Debug.WriteLine($"✅ Подключено к MySQL: {version}");
                }
            }

            // Запуск главного окна
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка инициализации приложения: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
