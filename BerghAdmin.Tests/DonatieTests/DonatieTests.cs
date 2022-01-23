using BerghAdmin.ApplicationServices.KentaaInterface;
using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
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
                .AddScoped<IDonatieService, DonatieService>()
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
                
                var donatieService = this.ServiceProvider?.GetRequiredService<IDonatieService>();
                if (donatieService != null)
                {
                    var fietsTochtDonations = kentaaDonations.Where(kd => kd.ProjectId == fietsTocht.KentaaProjectId);

                    // simulate that KentaaDonations read from Kentaa can be processed many times
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (var kentaaDonatie in fietsTochtDonations)
                        {
                            var result = donatieService.Save(donation);
                            Assert.AreEqual(ErrorCodeEnum.Ok, result);
                        }
                    }
                    var donaties = donatieService.GetAll<KentaaAction>();
                    Assert.IsTrue(donaties.Count() == fietsTochtDonations.Count());
                }

            }
        }

    }
}
