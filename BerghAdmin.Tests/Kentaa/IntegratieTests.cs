using BerghAdmin.ApplicationServices.KentaaInterface;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
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
        private IEvenementService? evenementService;
        private IKentaaInterfaceService? kentaaService;
        private IKentaaDonationService? kentaaDonatieService;
        private IKentaaActionService? actionService;
        private IKentaaProjectService? projectService;
        private IKentaaUserService? userService;

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

        [SetUp]
        public void Setup()
        {
            evenementService = this.ServiceProvider?.GetRequiredService<IEvenementService>();
            if (evenementService == null)
            {
                Assert.Fail("Can not instantiate evenement service");
                return;
            }

            kentaaService = this.ServiceProvider?.GetRequiredService<IKentaaInterfaceService>();
            if (kentaaService == null)
            {
                Assert.Fail("Can not instantiate Kentaa Interface service");
                return;
            }

            kentaaDonatieService = this.ServiceProvider?.GetRequiredService<IKentaaDonationService>();
            if (kentaaDonatieService == null)
            {
                Assert.Fail("Can not instantiate donatie service");
                return;
            }

            actionService = this.ServiceProvider?.GetRequiredService<IKentaaActionService>();
            if (actionService == null)
            {
                Assert.Fail("Can not instantiate action service");
                return;
            }

            projectService = this.ServiceProvider?.GetRequiredService<IKentaaProjectService>();
            if (projectService == null)
            {
                Assert.Fail("Can not instantiate project service");
                return;
            }

            userService = this.ServiceProvider?.GetRequiredService<IKentaaUserService>();
            if (userService == null)
            {
                Assert.Fail("Can not instantiate user service");
                return;
            }
        }

        [Test]
        public async Task ProcessKentaaActions()
        {
            await InsertInitialData();

            var actions = kentaaService!.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());
            await foreach (var action in actions)
            {
                actionService!.AddKentaaAction(action);
            }
            var kentaaActions = actionService!.GetAll();
            Assert.IsTrue(await actions.CountAsync() == kentaaActions?.Count());
        }

        [Test]
        public async Task ProcessKentaaProjects()
        {
            await InsertInitialData();

            var kentaaProjects = await GetProjectsFromKentaaAsync();

            foreach (var project in kentaaProjects)
            {
                projectService!.AddKentaaProject(project);
            }

            var projects = projectService!.GetAll();
            Assert.IsTrue(kentaaProjects.Count() == projects?.Count());
        }

        [Test]
        public async Task ProcessKentaaUsers()
        {
            await InsertInitialData();

            var users = kentaaService!.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter());

            await foreach (var user in users)
            {
                userService!.AddKentaaUser(user);
            }
            var kentaaUsers = userService!.GetAll();
            Assert.IsTrue(await users.CountAsync() == kentaaUsers?.Count());
        }

        [Test]
        public async Task FullKentaaIntegrationTest()
        {
            await InsertInitialData();

            // step 1: read all users and link to Personen
            var kentaaUsers = await kentaaService!.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter()).ToListAsync();
            userService!.AddKentaaUsers(kentaaUsers);

            // step 2: read projects and link to Evenementen
            var kentaaProjects = kentaaService.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
            projectService!.AddKentaaProjects(await kentaaProjects.ToListAsync());

            var fietsTochtNaam = "Hanzetocht 2023";
            if (evenementService!.GetByName(fietsTochtNaam) is not FietsTocht fietsTocht)
            {
                Assert.Fail($"Fietstocht {fietsTochtNaam} bestaat niet");
                return;
            }
            fietsTocht.Project = projectService
                                    .GetAll()?
                                    .Single(p => p.Titel == fietsTochtNaam);

            await evenementService.Save(fietsTocht);

            // step 3: read all actions and link to Personen
            var kentaaActions = await kentaaService.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter()).ToListAsync();
            actionService!.AddKentaaActions(kentaaActions);

            var persoonService = this.ServiceProvider?.GetRequiredService<IPersoonService>();
            if (persoonService == null)
            {
                Assert.Fail("persoon service is null");
                return;
            }

            foreach (var action in actionService.GetAll() ?? throw new ArgumentException("no actions"))
            {
                var persoon = persoonService.GetByActionId(action.Id);
                if (persoon == null)
                {
                    persoon = persoonService.GetByEmailAdres(action.EMail ?? "no-email");
                }
                if (persoon != null)
                {
                    persoon.DoneerActie = action;
                    persoonService.SavePersoon(persoon);
                }
            }

            // step 4: read all donations, create Donatie if needed and link to Personen
            var kentaaDonations = kentaaService.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
            kentaaDonatieService!.AddKentaaDonations(await kentaaDonations.ToListAsync());

            var donatieService = this.ServiceProvider?.GetRequiredService<IDonatieService>();
            if (donatieService == null)
            {
                Assert.Fail("donatie service is null");
                return;
            }
            foreach (var donation in kentaaDonatieService.GetAll() ?? throw new ArgumentException("no donations"))
            {
                if (donation.ActionId == 0)
                {
                    // niet gekoppeld aan een evenement
                    donatieService.AddKentaaDonatie(donation);
                    continue;
                }

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
            return await kentaaInterfaceService.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter()).ToListAsync();
        }
    }
}
