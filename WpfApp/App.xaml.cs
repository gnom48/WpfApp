using System;
using System.Windows;
using ClassLib.Config;
using ClassLib.models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using WpfApp.Data;
using WpfApp.mvvm;
using WpfApp.Windows;

namespace WpfApp;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDataServices();

                services.AddSingleton<MainWindowViewModel>();
                services.AddTransient<MainWindow>();
                services.AddTransient<EntityForm>();
            })
            .Build();

        ServiceProvider = AppHost.Services;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

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
                    System.Diagnostics.Debug.WriteLine($"Версия БД: {version}");

                    InitializeDatabase(sessionFactory);
                }
            }

            // Запуск главного окна
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost?.StopAsync()!;
        AppHost?.Dispose();

        base.OnExit(e);
    }

    #region test data

    /// <summary>
    /// Навайбкодил тестовых данных
    /// </summary>
    /// <param name="sessionFactory"></param>
    private void InitializeDatabase(ISessionFactory sessionFactory)
    {
        using (var session = sessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            try
            {
                ClearAllTables(session);

                SeedTestData(session);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    private void ClearAllTables(ISession session)
    {
        session.CreateSQLQuery("DELETE FROM `orders`").ExecuteUpdate();
        session.CreateSQLQuery("DELETE FROM `counterparties`").ExecuteUpdate();
        session.CreateSQLQuery("DELETE FROM `employees`").ExecuteUpdate();

        session.CreateSQLQuery("ALTER TABLE `orders` AUTO_INCREMENT = 1").ExecuteUpdate();
        session.CreateSQLQuery("ALTER TABLE `employees` AUTO_INCREMENT = 1").ExecuteUpdate();
        session.CreateSQLQuery("ALTER TABLE `counterparties` AUTO_INCREMENT = 1").ExecuteUpdate();
    }

    private void SeedTestData(ISession session)
    {
        var employees = new[]
        {
            new Employee { Surname = "Иванов", FirstName = "Иван", LastName = "Иванович", Position = Position.Руководитель, BirthDate = new DateTime(1985, 5, 15) },
            new Employee { Surname = "Петрова", FirstName = "Екатерина", LastName = "Сергеевна", Position = Position.Работник, BirthDate = new DateTime(1990, 8, 22) },
            new Employee { Surname = "Сидоров", FirstName = "Алексей", LastName = "Петрович", Position = Position.Работник, BirthDate = new DateTime(1988, 3, 10) }
        };

        foreach (var emp in employees)
        {
            try
            {
                session.Save(emp);
            } catch(Exception e) {
                Console.WriteLine(e); }
        }

        var counterparties = new[]
        {
            new Counterparty { Name = "ООО 'Ромашка'", Inn = "7708123456", Curator = employees[0] },
            new Counterparty { Name = "ОАО 'Кайф'", Inn = "7708654321", Curator = employees[1] },
            new Counterparty { Name = "ООО 'Рога и копыта'", Inn = "7708987654", Curator = employees[2] }
        };

        foreach (var cp in counterparties)
        {
            session.Save(cp);
        }

        var orders = new List<Order>();
        var now = DateTime.Now;

        foreach (var cp in counterparties)
        {
            for (int i = 0; i < 3; i++)
            {
                orders.Add(new Order
                {
                    Date = now.AddDays(-(i + 1) * 10),
                    Amount = (i + 1) * 10000 + 500.50m,
                    Employee = employees[i % employees.Length],
                    Counterparty = cp
                });
            }
        }

        foreach (var order in orders)
        {
            session.Save(order);
        }
    }
    #endregion
}