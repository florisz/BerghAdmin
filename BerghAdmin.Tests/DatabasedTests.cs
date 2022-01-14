using BerghAdmin.DbContexts;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Tests.TestHelpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

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
    }
}
