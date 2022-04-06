using BerghAdmin.DbContexts;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Tests.TestHelpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;
using System;

namespace BerghAdmin.Tests
{
    public abstract class DatabasedTests
    {
        private ServiceProvider? ServiceProvider;
        protected ApplicationDbContext? ApplicationDbContext;
        protected abstract void RegisterServices(ServiceCollection services);

        [SetUp]
        public void SetupTests()
        {
            // give each test its own separate database and service setup
            var connection = InMemoryDatabaseHelper.GetSqliteInMemoryConnection();

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
        }

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
