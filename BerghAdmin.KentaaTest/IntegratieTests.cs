using BerghAdmin.ApplicationServices.KentaaInterface;
using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Bihz;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.Tests.Kentaa
{
    [TestFixture]
    public class IntegratieTests : DatabaseTestSetup
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
                .AddUserSecrets<BerghAdmin.Program>()
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
                .Configure<SeedSettings>(databaseConfiguration.GetSection("Seeding"))
                .AddScoped<ISeedDataService, DebugSeedDataService>()
                .AddScoped<IEvenementService, EvenementService>()
                .AddScoped<IDonatieService, DonatieService>()
                .AddHttpClient()
                .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>()
                .Configure<KentaaConfiguration>(kentaaConfiguration.GetSection("KentaaConfiguration"))
            ;

            services.AddDbContext<ApplicationDbContext>(
                    options => options.UseMySql(GetDatabaseConnectionString(databaseConfiguration), ServerVersion.Parse("5.7"), po => po.EnableRetryOnFailure()));

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
            evenementService = GetRequiredService<IEvenementService>();
            kentaaService = GetRequiredService<IKentaaInterfaceService>();
            persoonService = GetRequiredService<IPersoonService>();
            bihzDonatieService = GetRequiredService<IBihzDonatieService>();
            bihzActionService = GetRequiredService<IBihzActieService>();
            bihzProjectService = GetRequiredService<IBihzProjectService>();
            bihzUserService = GetRequiredService<IBihzUserService>();
        }

        [Test]
        public async Task ProcessKentaaActions()
        {
            await InsertInitialData();

            var actions = kentaaService!.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());

            await foreach (var action in actions)
            {
                await bihzActionService!.AddAsync(action.Map());
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
                await bihzProjectService!.AddAsync(project.Map());
            }

            var projects = bihzProjectService!.GetAll();
            Assert.AreEqual(await kentaaProjects.CountAsync(), projects.Count());
            var aProject = projects.FirstOrDefault(p => p.Titel == "Fietstocht 2023");
            Assert.IsNotNull(aProject);
            Assert.IsTrue(aProject!.Gesloten == false);
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

            // actions must be known to read donations; otherwise the link with
            // the persoon can not be derived
            var actions = kentaaService!.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());

            await foreach (var action in actions)
            {
                await bihzActionService!.AddAsync(action.Map());
            }

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
            await bihzProjectService!.AddAsync(await kentaaProjects.Select(kp => kp.Map()).ToListAsync());

            var fietsTochtNaam = "Fietstocht 2023";
            if (evenementService!.GetByTitel(fietsTochtNaam) is not FietsTocht fietsTocht)
            {
                Assert.Fail($"Fietstocht {fietsTochtNaam} bestaat niet");
                return;
            }

            // step 3: read all actions and link to Personen
            var kentaaActions = await kentaaService.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter()).ToListAsync();
            await bihzActionService!.AddAsync(kentaaActions.Select(ka => ka.Map()));

            string[] emailAdresses = { "appie@aapnootmies.com", "bert@aapnootmies.com", "chappie@aapnootmies.com" };
            foreach (var emailAdress in emailAdresses)
            {
                var persoon = persoonService!.GetByEmailAdres(emailAdress);

                // check if the link to action is set for the three configured test persons
                Assert.IsNotNull(persoon, $"Persoon met email adres: -{emailAdress}- is niet bekend");
                Assert.IsTrue(persoon?.BihzActie != null);
            }

            // step 4: read all donations, create Donatie if needed and link to Personen
            var kentaaDonations = kentaaService.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
            bihzDonatieService!.Add(await kentaaDonations.Select(kd => kd.Map()).ToListAsync());

            // check if persoon chappie has donaties
            var chappie = persoonService!.GetByEmailAdres("chappie@aapnootmies.com");
            Assert.IsNotNull(chappie, "Persoon met email adres: -chappie@aapnootmies.com- is niet bekend");
            Assert.IsTrue(chappie?.BihzActie != null, "Persoon heeft geen gelinkte Action in Kentaa");
            Assert.True(chappie!.Donaties != null, "Geen Donaties voor test persoon Chappie");
            Assert.True(chappie!.Donaties!.Sum(d => d.Bedrag) >= 75, "Chappie moet minimaal 75 euro opgehaald hebben");

            Assert.Pass();
        }

        private async Task InsertInitialData()
        {
            var seedDataService = GetRequiredService<ISeedDataService>();
            await seedDataService.SeedInitialData();
        }
    }
}
