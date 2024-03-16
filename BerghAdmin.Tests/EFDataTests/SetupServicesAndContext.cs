using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Tests.TestHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;

namespace BerghAdmin.Tests
{
    public abstract class SetupServicesAndContext
    {
        private ServiceProvider? _serviceProvider;

        protected abstract void RegisterServices(ServiceCollection services);
        protected abstract void RegisterDatabaseAccess(IServiceCollection services);

        [SetUp]
        public void SetupTests()
        {
            var services = new ServiceCollection();
            RegisterDatabaseAccess(services);
            RegisterServices(services);

            _serviceProvider = services.BuildServiceProvider();

            // test if we can create the context and database connection can be setup successfully
            var contextFactory = _serviceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            using var context = contextFactory.CreateDbContext();
            {
                context?.Database.OpenConnection();
                context?.Database.EnsureCreated();
            }
        }

        // Helper function to avoid warnings in unit tests
        public T GetRequiredService<T>() where T:notnull
        {
            if (_serviceProvider == null)
                throw new ArgumentNullException(nameof(_serviceProvider), "Test SetUp should run first");

            var serviceInstance = _serviceProvider!.GetRequiredService<T>();
            if (serviceInstance == null)
                throw new ArgumentNullException(nameof(serviceInstance), $"Process not configured properly; cannot instantiate {typeof(T).Name}");

            return serviceInstance;
        }
    }
}
