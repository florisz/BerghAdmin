using BerghAdmin.ApplicationServices.KentaaInterface;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Bihz;
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
        private IBihzDonatieService? bihzDonatieService;
        private IBihzActieService? bihzActionService;
        private IBihzProjectService? bihzProjectService;
        private IBihzUserService? bihzUserService;
        private IPersoonService? persoonService;
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
                .AddScoped<IBihzActieService, BihzActieService>()
                .AddScoped<IBihzDonatieService, BihzDonatieService>()
                .AddScoped<IBihzProjectService, BihzProjectService>()
                .AddScoped<IBihzUserService, BihzUserService>()
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

        static string GetDatabaseConnectionString(IConfigurationRoot configuration)
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

            persoonService = this.ServiceProvider?.GetRequiredService<IPersoonService>();
            if (persoonService == null)
            {
                Assert.Fail("Can not instantiate Kentaa Interface service");
                return;
            }

            bihzDonatieService = this.ServiceProvider?.GetRequiredService<IBihzDonatieService>();
            if (bihzDonatieService == null)
            {
                Assert.Fail("Can not instantiate donatie service");
                return;
            }

            bihzActionService = this.ServiceProvider?.GetRequiredService<IBihzActieService>();
            if (bihzActionService == null)
            {
                Assert.Fail("Can not instantiate action service");
                return;
            }

            bihzProjectService = this.ServiceProvider?.GetRequiredService<IBihzProjectService>();
            if (bihzProjectService == null)
            {
                Assert.Fail("Can not instantiate project service");
                return;
            }

            bihzUserService = this.ServiceProvider?.GetRequiredService<IBihzUserService>();
            if (bihzUserService == null)
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
                bihzActionService!.Add(action.Map());
            }
            var bihzActions = bihzActionService!.GetAll();
            Assert.IsTrue(await actions.CountAsync() == bihzActions?.Count());
        }

        [Test]
        public async Task ProcessKentaaProjects()
        {
            await InsertInitialData();

            var kentaaProjects = kentaaService!.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());

            await foreach (var project in kentaaProjects)
            {
                bihzProjectService!.Add(project.Map());
            }

            var projects = bihzProjectService!.GetAll();
            Assert.AreEqual(await kentaaProjects.CountAsync(), projects?.Count());
        }

        [Test]
        public async Task ProcessKentaaUsers()
        {
            await InsertInitialData();

            var users = kentaaService!.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter());

            await foreach (var user in users)
            {
                bihzUserService!.Add(user.Map());
            }
            var bihzUsers = bihzUserService!.GetAll();
            Assert.IsTrue(await users.CountAsync() == bihzUsers?.Count());
        }

        [Test]
        public async Task ProcessKentaaDonations()
        {
            await InsertInitialData();

            var donations = kentaaService!.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());

            await foreach (var donation in donations)
            {
                bihzDonatieService!.Add(donation.Map());
            }
            var bihzDonaties = bihzDonatieService!.GetAll();
            Assert.IsTrue(await donations.CountAsync() == bihzDonaties?.Count());
        }

        [Test]
        public async Task FullKentaaIntegrationTest()
        {
            await InsertInitialData();

            // step 1: read all users and link to Personen
            var kentaaUsers = await kentaaService!.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter()).ToListAsync();
            bihzUserService!.Add(kentaaUsers.Select(ku => ku.Map()));

            // step 2: read projects and link to Evenementen
            var kentaaProjects = kentaaService.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
            bihzProjectService!.Add(await kentaaProjects.Select(kp => kp.Map()).ToListAsync());

            var fietsTochtNaam = "Hanzetocht 2023";
            if (evenementService!.GetByTitel(fietsTochtNaam) is not FietsTocht fietsTocht)
            {
                Assert.Fail($"Fietstocht {fietsTochtNaam} bestaat niet");
                return;
            }

            // step 3: read all actions and link to Personen
            var kentaaActions = await kentaaService.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter()).ToListAsync();
            bihzActionService!.Add(kentaaActions.Select(ka => ka.Map()));

            foreach (var action in bihzActionService.GetAll() ?? throw new ArgumentException("no actions"))
            {
                var persoon = persoonService!.GetByActionId(action.Id);
                //if (persoon == null)
                //{
                //    persoon = persoonService.GetByEmailAdres(action.Email ?? "no-email");
                //}
                //if (persoon != null)
                //{
                //    persoon.BihzActie = action;
                //    persoonService.SavePersoon(persoon);
                //}
            }

            // step 4: read all donations, create Donatie if needed and link to Personen
            var kentaaDonations = kentaaService.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
            bihzDonatieService!.Add(await kentaaDonations.Select(kd => kd.Map()).ToListAsync());

            var donatieService = this.ServiceProvider?.GetRequiredService<IDonatieService>();
            if (donatieService == null)
            {
                Assert.Fail("donatie service is null");
                return;
            }
            foreach (var donation in bihzDonatieService.GetAll() ?? throw new ArgumentException("no donations"))
            {
                if (donation.ActionId == 0)
                {
                    // niet gekoppeld aan een evenement
                    donatieService.ProcessBihzDonatie(donation);
                    continue;
                }

                var persoon = persoonService!.GetByActionId(donation.ActionId);
                if (persoon != null)
                {
                    var donaties = persoon.Donaties;
                    if (donaties?.FirstOrDefault(d => d.KentaaDonatie?.DonationId == donation.Id) == null)
                    {
                        donatieService.ProcessBihzDonatie(donation, persoon);
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
    }
}
