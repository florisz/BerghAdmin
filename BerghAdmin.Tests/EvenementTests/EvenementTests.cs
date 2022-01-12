using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerghAdmin.Tests.EvenementTests
{
    [TestFixture]
    public class EvenementTests
    {
        private ServiceProvider _serviceProvider;
        private ApplicationDbContext _applicationDbContext;


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
                }, ServiceLifetime.Singleton, ServiceLifetime.Singleton)
                .AddScoped<IEvenementService, EvenementService>();

            _serviceProvider = services.BuildServiceProvider();

            var scope = _serviceProvider.CreateScope();
            _applicationDbContext = scope?.ServiceProvider.GetService<ApplicationDbContext>();

            _applicationDbContext?.Database.OpenConnection();
            _applicationDbContext?.Database.EnsureCreated();
        }

        [Test]
        public void GetByNameTest()
        {
            const string fietsTochtNaam = "Fietstocht1";

            var service = _serviceProvider.GetRequiredService<IEvenementService>();
            service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByName(fietsTochtNaam);

            Assert.AreEqual(fietsTocht.Naam, fietsTochtNaam);
        }

        [Test]
        public void GetByIdTest()
        {
            const string fietsTochtNaam = "Fietstocht2";

            var service = _serviceProvider.GetRequiredService<IEvenementService>();
            service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByName(fietsTochtNaam);

            Assert.IsNotNull(fietsTocht);

            var fietsTochtById = service.GetById(fietsTocht.Id);

            Assert.AreEqual(fietsTochtById.Naam, fietsTochtNaam);
        }

        [Test]
        public void AddWithExistingName()
        {
            const string fietsTochtNaam = "Fietstocht3";

            var service = _serviceProvider.GetRequiredService<IEvenementService>();
            service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var errorCode = service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2023, 1, 1) });

            Assert.AreEqual(errorCode, ErrorCodeEnum.Conflict);
        }

        [Test]
        public void UpdateEvenement()
        {
            const string fietsTochtNaam = "Fietstocht4";
            const string fietsTochtUpdatedNaam = "Fietstocht4.1";

            var service = _serviceProvider.GetRequiredService<IEvenementService>();
            var fietsTocht = new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) };
            service.SaveEvenement(fietsTocht);
            
            fietsTocht.Naam = fietsTochtUpdatedNaam;
            service.SaveEvenement(fietsTocht);

            var fietsTochtById = service.GetById(fietsTocht.Id);

            Assert.AreEqual(fietsTochtById.Naam, fietsTochtUpdatedNaam);
        }

        [Test]
        public void GetAllEvenementen()
        {
            var service = _serviceProvider.GetRequiredService<IEvenementService>();

            var strArray = new string[] { "aap", "noot", "mies" };
            foreach (var name in strArray)
            {
                var fietsTocht = new FietsTocht() { Naam = name, GeplandJaar = new DateTime(2022, 1, 1) };
                service.SaveEvenement(fietsTocht);
            }

            strArray = new string[] { "wim", "zus", "jet" };
            foreach (var name in strArray)
            {
                var golfDag = new GolfDag() { Naam = name, GeplandeDatum = new DateTime(2022, 1, 1) };
                service.SaveEvenement(golfDag);
            }

            var fietsTochten = service.GetAllEvenementen<FietsTocht>();
            var golfDagen = service.GetAllEvenementen<GolfDag>();

            Assert.AreEqual(3, fietsTochten?.ToList().Count);
            Assert.AreEqual(3, golfDagen?.ToList().Count);
        }
    }
}
