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
        protected ServiceProvider? ServiceProvider;
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
        internal T GetRequiredService<T>()
        {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            T? serviceInstance = ServiceProvider.GetRequiredService<T>();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
#pragma warning restore CS8604 // Possible null reference argument.

            return serviceInstance;
        }
    }
}
