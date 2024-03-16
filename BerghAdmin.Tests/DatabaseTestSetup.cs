using BerghAdmin.DbContexts;
using BerghAdmin.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace BerghAdmin.Tests
{
    public abstract class DatabaseTestSetup
    {
        private ServiceProvider? ServiceProvider;
        protected ApplicationDbContext? ApplicationDbContext;

        protected abstract void RegisterServices(ServiceCollection services);

        [SetUp]
        public void SetupTests()
        {
            // give each test its own separate database and service setup
            var connection = InMemoryDatabaseHelper.GetSqliteInMemoryConnection();

            // creater a host builder to get the configuration
            var host = CreateHostBuilder().Build();
            var services = new ServiceCollection();
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<ApplicationDbContext>(builder =>
                {
                    builder.UseSqlite(connection);
                }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);
            
            RegisterServices(services);

            ServiceProvider = services.BuildServiceProvider();

            ApplicationDbContext = ServiceProvider.GetRequiredService<ApplicationDbContext>();

            ApplicationDbContext?.Database.OpenConnection();
            ApplicationDbContext?.Database.EnsureCreated();

            // get secret value from configuration
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var syncFusionLicenseKey = configuration.GetValue<string>("SyncfusionConfiguration:LicenseKey");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncFusionLicenseKey);
        }

        public static IHostBuilder CreateHostBuilder(string[]? args = null) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                    config.AddUserSecrets<Program>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                });

        // Helper function to avoid warnings in unit tests
        public T GetRequiredService<T>() where T:notnull
        {
            if (ServiceProvider == null)
                throw new ArgumentNullException(nameof(ServiceProvider), "Test SetUp should run first");

            var serviceInstance = ServiceProvider!.GetRequiredService<T>();
            if (serviceInstance == null)
                throw new ArgumentNullException(nameof(serviceInstance), $"Process not configured properly; cannot instantiate {typeof(T).Name}");

            return serviceInstance;
        }
    }
}
