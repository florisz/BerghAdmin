using BerghAdmin.ApplicationServices.KentaaInterface;
using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Kentaa;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace BerghAdmin.Tests.DonatieTests
{
    [TestFixture]
    public class DonatieTests : DatabasedTests
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            var kentaaConfiguration = new ConfigurationBuilder()
                .AddUserSecrets<KentaaConfiguration>()
                .Build();
            var databaseConfiguration = new ConfigurationBuilder()
                .AddUserSecrets<DatabaseConfiguration>()
                .Build();

            services
                .AddOptions()
                .AddScoped<IKentaaActionService, KentaaActionService>()
                .AddScoped<IKentaaDonationService, KentaaDonationService>()
                .AddScoped<IKentaaProjectService, KentaaProjectService>()
                .AddScoped<IKentaaUserService, KentaaUserService>()
                .AddScoped<IEvenementService, EvenementService>()
                .AddScoped<IRolService, RolService>()
                .AddScoped<IPersoonService, PersoonService>()
                .AddScoped<ISeedDataService, SeedDataService>()
                .AddScoped<IEvenementService, EvenementService>()
                .AddHttpClient()
                .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>()
                .Configure<KentaaConfiguration>(kentaaConfiguration.GetSection("KentaaConfiguration"))
            ;

            services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(GetDatabaseConnectionString(databaseConfiguration), po => po.EnableRetryOnFailure()));

        }

        string GetDatabaseConnectionString(IConfigurationRoot configuration)
        {
            var databaseConfiguration = configuration.GetSection("DatabaseConfiguration").Get<DatabaseConfiguration>();
            if (databaseConfiguration == null)
            {
                throw new ApplicationException("Secrets for Database access (connection string & password) can not be found in configuration");
            }
            return databaseConfiguration.ConnectionString ?? throw new ArgumentException("ConnectionString not specified");
        }

        [Test]
        public async Task ProcessKentaaDonations()
        {
            var seedService = this.ServiceProvider?.GetService<ISeedDataService>();
            if (seedService == null)
            {
                Assert.Fail();
                return;
            }
            await seedService.SeedInitialData();

            var service = this.ServiceProvider?.GetRequiredService<IEvenementService>();
            if (service == null)
            {
                Assert.Fail("Can not instantiate evenement service");
                return;
            }

            var f = service.GetByName("Hanzetocht");
            var fietsTocht = f as FietsTocht;
            if (fietsTocht == null)
            {
                return;
            }

            var kentaaService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaService != null)
            {
                var filter = new KentaaFilter()
                {
                    StartAt = 1,
                    PageSize = 25
                };
                var kentaaDonations = await kentaaService.GetDonationsByQuery(filter);
                
                var donatieService = this.ServiceProvider?.GetRequiredService<IKentaaDonationService>();
                if (donatieService != null)
                {
                    var fietsTochtDonations = kentaaDonations.Where(kd => kd.ProjectId == fietsTocht.KentaaProjectId);

                    // simulate that KentaaDonations read from Kentaa can be processed many times
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (var kentaaDonatie in fietsTochtDonations)
                        {
                            donatieService.AddKentaaDonation(kentaaDonatie);
                        }
                    }
                    var donaties = donatieService.GetAll();
                    Assert.IsTrue(donaties.Count() == fietsTochtDonations.Count());
                }

            }
        }

        [Test]
        public async Task ProcessKentaaActions()
        {
            var seedService = this.ServiceProvider?.GetService<ISeedDataService>();
            if (seedService == null)
            {
                Assert.Fail();
                return;
            }
            await seedService.SeedInitialData();

            var kentaaInterfaceService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaInterfaceService != null)
            {
                var actions = await kentaaInterfaceService.GetActionsByQuery(new KentaaFilter());

                var actionService = this.ServiceProvider?.GetRequiredService<IKentaaActionService>();
                if (actionService != null)
                {
                    foreach (var action in actions)
                    {
                        actionService.AddKentaaAction(action);
                    }
                    var kentaaActions = actionService.GetAll();
                    Assert.IsTrue(actions.Count() == kentaaActions.Count());
                }

            }
        }

        [Test]
        public async Task ProcessKentaaProjects()
        {
            var seedService = this.ServiceProvider?.GetService<ISeedDataService>();
            if (seedService == null)
            {
                Assert.Fail();
                return;
            }
            await seedService.SeedInitialData();

            var kentaaInterfaceService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaInterfaceService != null)
            {
                var projects = await kentaaInterfaceService.GetProjectsByQuery(new KentaaFilter());

                var projectService = this.ServiceProvider?.GetRequiredService<IKentaaProjectService>();
                if (projectService != null)
                {
                    foreach (var project in projects)
                    {
                        projectService.AddKentaaProject(project);
                    }
                    var kentaaProjects = projectService.GetAll();
                    Assert.IsTrue(projects.Count() == kentaaProjects.Count());
                }

            }
        }

        [Test]
        public async Task ProcessKentaaUsers()
        {
            var seedService = this.ServiceProvider?.GetService<ISeedDataService>();
            if (seedService == null)
            {
                Assert.Fail();
                return;
            }
            await seedService.SeedInitialData();

            var kentaaInterfaceService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaInterfaceService != null)
            {
                var users = await kentaaInterfaceService.GetUsersByQuery(new KentaaFilter());

                var userService = this.ServiceProvider?.GetRequiredService<IKentaaUserService>();
                if (userService != null)
                {
                    foreach (var user in users)
                    {
                        userService.AddKentaaUser(user);
                    }
                    var kentaaUsers = userService.GetAll();
                    Assert.IsTrue(users.Count() == kentaaUsers.Count());
                }

            }
        }

    }
}
