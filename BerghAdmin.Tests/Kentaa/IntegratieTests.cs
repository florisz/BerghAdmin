using BerghAdmin.ApplicationServices.KentaaInterface;
using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Kentaa;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;
using BerghAdmin.Services.Donaties;

namespace BerghAdmin.Tests.Kentaa
{
    [TestFixture]
    public class IntegratieTests : DatabasedTests
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
                .AddScoped<IDonatieService, DonatieService>()
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
            await InsertInitialData();

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
                var kentaaDonations = await kentaaService.GetKentaaIssuesByQuery<KM.Donations, KM.Donation>(filter);
                
                var donatieService = this.ServiceProvider?.GetRequiredService<IKentaaDonationService>();
                if (donatieService != null)
                {
                    var fietsTochtDonations = kentaaDonations.Where(kd => kd.ProjectId == fietsTocht.Project.ProjectId);

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
            await InsertInitialData();

            var kentaaInterfaceService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaInterfaceService != null)
            {
                var actions = await kentaaInterfaceService.GetKentaaIssuesByQuery<KM.Actions, KM.Action>(new KentaaFilter());

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
            await InsertInitialData();

            var kentaaProjects = await GetProjectsFromKentaaAsync();

            var projectService = this.ServiceProvider?.GetRequiredService<IKentaaProjectService>();
            if (projectService != null)
            {
                foreach (var project in kentaaProjects)
                {
                    projectService.AddKentaaProject(project);
                }

                var projects = projectService.GetAll();
                Assert.IsTrue(kentaaProjects.Count() == kentaaProjects.Count());
            }
        }

        [Test]
        public async Task ProcessKentaaUsers()
        {
            await InsertInitialData();

            var kentaaInterfaceService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaInterfaceService != null)
            {
                var users = await kentaaInterfaceService.GetKentaaIssuesByQuery<KM.Users, KM.User>(new KentaaFilter());

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

        [Test]
        public async Task FullKentaaIntegrationTest()
        {
            await InsertInitialData();

            // step 1: read all users and link to Personen
            var kentaaInterfaceService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaInterfaceService == null)
            {
                Assert.Fail("kentaa interface service is null");
                return;
            }
            
            var kentaaUsers = await kentaaInterfaceService.GetKentaaIssuesByQuery<KM.Users, KM.User>(new KentaaFilter());
            var userService = this.ServiceProvider?.GetRequiredService<IKentaaUserService>();
            if (userService == null)
            {
                Assert.Fail("user service is null");
                return;
            }
            userService.AddKentaaUsers(kentaaUsers);

            // step 2: read projects and link to Evenementen
            var kentaaProjects = await kentaaInterfaceService.GetKentaaIssuesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
            var projectService = this.ServiceProvider?.GetRequiredService<IKentaaProjectService>();
            if (projectService == null)
            {
                Assert.Fail("project service is null");
                return;
            }
            projectService.AddKentaaProjects(kentaaProjects);
            var evenementService = this.ServiceProvider?.GetRequiredService<IEvenementService>();
            if (evenementService == null)
            {
                Assert.Fail("evenement service is null");
                return;
            }
            var fietsTochtNaam = "Hanzetocht 2023";
            var fietsTocht = evenementService.GetByName(fietsTochtNaam) as FietsTocht;
            if (fietsTocht == null)
            {
                Assert.Fail($"Fietstocht {fietsTochtNaam} bestaat niet");
                return;
            }
            fietsTocht.Project = projectService
                                    .GetAll()
                                    .SingleOrDefault(p => p.Titel == fietsTochtNaam);
            if (fietsTocht.Project != null)
            {
                evenementService.Save(fietsTocht);
            }

            // step 3: read all actions and link to Personen
            var kentaaActions = await kentaaInterfaceService.GetKentaaIssuesByQuery<KM.Actions, KM.Action>(new KentaaFilter());
            var actionService = this.ServiceProvider?.GetRequiredService<IKentaaActionService>();
            if (actionService == null)
            {
                Assert.Fail("action service is null");
                return;
            }
            actionService.AddKentaaActions(kentaaActions);
            var persoonService = this.ServiceProvider?.GetRequiredService<IPersoonService>();
            if (persoonService == null)
            {
                Assert.Fail("persoon service is null");
                return;
            }
            foreach (var action in actionService.GetAll())
            {
                var persoon = persoonService.GetByActionId(action.Id);
                if (persoon == null)
                {
                    persoon = persoonService.GetByEmailAdres(action?.EMail);
                }
                if (persoon != null)
                {
                    persoon.KentaaAction = action;
                    persoonService.SavePersoon(persoon);
                }
            }

            // step 4: read all donations, create Donatie if needed and link to Personen
            var kentaaDonations = await kentaaInterfaceService.GetKentaaIssuesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
            var donationService = this.ServiceProvider?.GetRequiredService<IKentaaDonationService>();
            if (donationService == null)
            {
                Assert.Fail("donation service is null");
                return;
            }
            donationService.AddKentaaDonations(kentaaDonations);
            var donatieService = this.ServiceProvider?.GetRequiredService<IDonatieService>();
            if (donatieService == null)
            {
                Assert.Fail("donatie service is null");
                return;
            }
            foreach (var donation in donationService.GetAll())
            {
                if (donation.ActionId == 0)
                {
                    // niet gekoppeld aan een evenement
                    donatieService.AddKentaaDonatie(donation);
                }
                else
                {
                    var persoon = persoonService.GetByActionId(donation.ActionId);
                    if (persoon != null)
                    {
                        var donaties = persoon.Donaties;
                        if (donaties?.FirstOrDefault(d => d.KentaaDonatie?.DonationId == donation.Id) == null)
                        {
                            donatieService.AddKentaaDonatie(donation, persoon);
                        }
                    }
                }
            }

            Assert.Pass();
        }

        private async Task InsertInitialData()
        {
            var seedService = this.ServiceProvider?.GetService<ISeedDataService>();
            if (seedService == null)
            {
                Assert.Fail();
                return;
            }

            await seedService.SeedInitialData();
        }

        private async Task<IEnumerable<KM.Project>> GetProjectsFromKentaaAsync()
        {
            var kentaaInterfaceService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaInterfaceService == null)
            {
                // for ease of use return an ampty list
                return new List<KM.Project>();
            }
            return await kentaaInterfaceService.GetKentaaIssuesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
        }

        private async Task<IEnumerable<KM.User>> GetUsersFromKentaaAsync()
        {
            var kentaaInterfaceService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaInterfaceService == null)
            {
                // for ease of use return an ampty list
                return new List<KM.User>();
            }
            return await kentaaInterfaceService.GetKentaaIssuesByQuery<KM.Users, KM.User>(new KentaaFilter());
        }

    }
}
