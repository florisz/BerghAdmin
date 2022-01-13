using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using System;
using System.Linq;

namespace BerghAdmin.Tests.EvenementTests
{
    [TestFixture]
    public class EvenementTests : DatabasedTests
    {
        private ServiceProvider ServiceProvider { get; set; }
        protected override void RegisterServices(ServiceCollection services)
        {
            // give each test its own separate database and service setup
            var connection = InMemoryDatabaseHelper.GetSqliteInMemoryConnection();
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddEntityFrameworkSqlite()
                .AddDbContext<ApplicationDbContext>(builder =>
                {
                    builder.UseSqlite(connection);
                }, ServiceLifetime.Singleton, ServiceLifetime.Singleton)
                .AddScoped<IEvenementService, EvenementService>()
                .AddScoped<IPersoonService, PersoonService>();

            this.ServiceProvider = services.BuildServiceProvider();

            var scope = this.ServiceProvider.CreateScope();
            var _applicationDbContext = scope?.ServiceProvider.GetService<ApplicationDbContext>();

            _applicationDbContext?.Database.OpenConnection();
            _applicationDbContext?.Database.EnsureCreated();
        }

        [Test]
        public void GetByNameTest()
        {
            const string fietsTochtNaam = "Fietstocht1";

            var service = this.ServiceProvider.GetRequiredService<IEvenementService>();
            service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByName(fietsTochtNaam);

            Assert.AreEqual(fietsTocht.Naam, fietsTochtNaam);
        }

        [Test]
        public void GetByIdTest()
        {
            const string fietsTochtNaam = "Fietstocht2";

            var service = this.ServiceProvider.GetRequiredService<IEvenementService>();
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

            var service = this.ServiceProvider.GetRequiredService<IEvenementService>();
            service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var errorCode = service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2023, 1, 1) });

            Assert.AreEqual(errorCode, ErrorCodeEnum.Conflict);
        }

        [Test]
        public void UpdateEvenement()
        {
            const string fietsTochtNaam = "Fietstocht4";
            const string fietsTochtUpdatedNaam = "Fietstocht4.1";

            var service = this.ServiceProvider.GetRequiredService<IEvenementService>();
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
            var service = this.ServiceProvider.GetRequiredService<IEvenementService>();

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

        [Test]
        public void AddPersoon()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.ServiceProvider.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) };
            service.SaveEvenement(fietsTocht);
            service.AddDeelnemer(fietsTocht, new Persoon() { EmailAdres = "aap@noot.com" } );
            service = null;

            var service2 = this.ServiceProvider.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            var persoon = fietsTochtById?.Deelnemers.FirstOrDefault();
            var isDeelnemerVan = persoon?.IsDeelnemerVan?.FirstOrDefault(f => f.Id == fietsTocht.Id) != null;

            Assert.IsNotNull(fietsTochtById);
            Assert.AreEqual(1, fietsTochtById?.Deelnemers.Count);
            Assert.IsTrue(isDeelnemerVan);
        }

        [Test]
        public void DeletePersoon()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.ServiceProvider.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) };
            service.SaveEvenement(fietsTocht);
            service.AddDeelnemer(fietsTocht, new Persoon() { EmailAdres = "aap@noot.com" });
            service = null;

            var service2 = this.ServiceProvider.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            var persoon = fietsTochtById?.Deelnemers.FirstOrDefault();
            Assert.IsNotNull(fietsTochtById);
            Assert.IsNotNull(persoon);

            // try to delete an exisitng deelnemer from the evenement
            var result = service2.DeleteDeelnemer(fietsTochtById, persoon);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, fietsTochtById.Deelnemers?.Count());

            // try to delete a non exisitng deelnemer from the evenement
            result = service2.DeleteDeelnemer(fietsTochtById, persoon);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }

        [Test]
        public void DeletePersoonById()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.ServiceProvider.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) };
            service.SaveEvenement(fietsTocht);
            var persoon = new Persoon() { EmailAdres = "aap@noot.com" };
            service.AddDeelnemer(fietsTocht, persoon);
        
            service = null;

            var service2 = this.ServiceProvider.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            Assert.IsNotNull(fietsTochtById);

            // try to delete an exisitng deelnemer from the evenement
            var result = service2.DeleteDeelnemer(fietsTochtById, persoon.Id);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, fietsTochtById.Deelnemers?.Count());

            // try to delete a non exisitng deelnemer from the evenement
            result = service2.DeleteDeelnemer(fietsTochtById, persoon);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }
    }
}
