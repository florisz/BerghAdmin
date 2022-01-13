using BerghAdmin.Data;
using BerghAdmin.General;
using BerghAdmin.Services.Evenementen;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using System;
using System.Linq;

namespace BerghAdmin.Tests.EvenementTests
{
    [TestFixture]
    public class EvenementTests : DatabasedTests
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            services.AddScoped<IEvenementService, EvenementService>();
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
    }
}
