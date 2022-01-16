using BerghAdmin.Data;
using BerghAdmin.General;
using BerghAdmin.Services;
using BerghAdmin.Services.Evenementen;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace BerghAdmin.Tests.EvenementTests
{
    [TestFixture]
    public class DonatieTests : DatabasedTests
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            services
                .AddScoped<IEvenementService, EvenementService>()
                .AddScoped<IPersoonService, PersoonService>();
        }

        [Test]
        public void GetByNameTest()
        {
            const string fietsTochtNaam = "Fietstocht1";

            var service = this.GetRequiredService<IEvenementService>();
            service.Save(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByName(fietsTochtNaam);

            Assert.AreEqual(fietsTocht?.Naam, fietsTochtNaam);
        }

        [Test]
        public void GetByIdTest()
        {
            const string fietsTochtNaam = "Fietstocht2";

            var service = this.GetRequiredService<IEvenementService>();
            service.Save(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByName(fietsTochtNaam);

            Assert.IsNotNull(fietsTocht);
            if (fietsTocht != null)
            {
                var fietsTochtById = service.GetById(fietsTocht.Id);
                Assert.IsNotNull(fietsTochtById);
                // not really necessary but to avoid warnings
                if (fietsTochtById != null)
                {
                    Assert.AreEqual(fietsTochtById.Naam, fietsTochtNaam);
                }
            }
        }

        [Test]
        public async Task AddWithExistingName()
        {
            const string fietsTochtNaam = "Fietstocht3";

            var service = this.GetRequiredService<IEvenementService>();
            await service.Save(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var errorCode = await service.Save(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2023, 1, 1) });

            Assert.AreEqual(errorCode, ErrorCodeEnum.Conflict);
        }

        [Test]
        public void UpdateEvenement()
        {
            const string fietsTochtNaam = "Fietstocht4";
            const string fietsTochtUpdatedNaam = "Fietstocht4.1";

            var service = this.GetRequiredService<IEvenementService>();
            var fietsTocht = new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) };
            service.Save(fietsTocht);

            fietsTocht.Naam = fietsTochtUpdatedNaam;
            service.Save(fietsTocht);

            var fietsTochtById = service.GetById(fietsTocht.Id);
            Assert.IsNotNull(fietsTochtById);

            // not really necessary but to avoid warnings
            if (fietsTochtById != null)
            {
                Assert.AreEqual(fietsTochtById.Naam, fietsTochtUpdatedNaam);
            }
        }

        [Test]
        public void GetAll()
        {
            var service = this.GetRequiredService<IEvenementService>();

            var strArray = new string[] { "aap", "noot", "mies" };
            foreach (var name in strArray)
            {
                var fietsTocht = new FietsTocht() { Naam = name, GeplandJaar = new DateTime(2022, 1, 1) };
                service.Save(fietsTocht);
            }

            strArray = new string[] { "wim", "zus", "jet" };
            foreach (var name in strArray)
            {
                var golfDag = new GolfDag() { Naam = name, GeplandeDatum = new DateTime(2022, 1, 1) };
                service.Save(golfDag);
            }

            var fietsTochten = service.GetAll<FietsTocht>();
            var golfDagen = service.GetAll<GolfDag>();

            Assert.AreEqual(3, fietsTochten?.ToList().Count);
            Assert.AreEqual(3, golfDagen?.ToList().Count);
        }

        [Test]
        public void AddPersoon()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) };
            service.Save(fietsTocht);
            service.AddDeelnemer(fietsTocht, new Persoon() { EmailAdres = "aap@noot.com" });
            service = null;

            var service2 = this.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            var persoon = fietsTochtById?.Deelnemers.FirstOrDefault();
            var isDeelnemerVan = persoon?.IsDeelnemerVan?.FirstOrDefault(f => f.Id == fietsTocht.Id) != null;

            Assert.IsNotNull(fietsTochtById);
            Assert.AreEqual(1, fietsTochtById?.Deelnemers.Count);
            Assert.IsTrue(isDeelnemerVan);
        }

        [Test]
        public async Task DeletePersoon()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) };
            await service.Save(fietsTocht);
            await service.AddDeelnemer(fietsTocht, new Persoon() { EmailAdres = "aap@noot.com" });
            
            var service2 = this.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            var persoon = fietsTochtById?.Deelnemers.FirstOrDefault();
            Assert.IsNotNull(fietsTochtById);
            Assert.IsNotNull(persoon);

            // not really necessary but to avoid warnings
            if (fietsTochtById != null && persoon != null)
            {
                // try to delete an exisitng deelnemer from the evenement
                var result = await service2.DeleteDeelnemer(fietsTochtById, persoon);
                Assert.AreEqual(ErrorCodeEnum.Ok, result);
                Assert.AreEqual(0, fietsTochtById.Deelnemers?.Count());

                // try to delete a non exisitng deelnemer from the evenement
                result = await service2.DeleteDeelnemer(fietsTochtById, persoon);
                Assert.AreEqual(ErrorCodeEnum.Ok, result);
            }
        }

        [Test]
        public async Task DeletePersoonById()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) };
            await service.Save(fietsTocht);
            var persoon = new Persoon() { EmailAdres = "aap@noot.com" };
            await service.AddDeelnemer(fietsTocht, persoon);

            service = null;

            var service2 = this.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            Assert.IsNotNull(fietsTochtById);

            // not really necessary but to avoid warnings
            if (fietsTochtById != null)
            {
                // try to delete an exisitng deelnemer from the evenement
                var result = await service2.DeleteDeelnemer(fietsTochtById, persoon.Id);
                Assert.AreEqual(ErrorCodeEnum.Ok, result);
                Assert.AreEqual(0, fietsTochtById.Deelnemers?.Count());

                // try to delete a non exisitng deelnemer from the evenement
                result = await service2.DeleteDeelnemer(fietsTochtById, persoon);
                Assert.AreEqual(ErrorCodeEnum.Ok, result);
            }
        }
    }
}
