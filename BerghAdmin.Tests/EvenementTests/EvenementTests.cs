using BerghAdmin.Data;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;
using BerghAdmin.Services;
using BerghAdmin.Services.Evenementen;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BerghAdmin.Tests.EvenementTests
{
    [TestFixture]
    public class DonatieTests : DatabaseTestSetup
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            services
                .AddScoped<IEvenementService, EvenementService>()
                .AddScoped<IPersoonService, PersoonService>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }

        [Test]
        public async Task GetByNameTest()
        {
            const string fietsTochtNaam = "Fietstocht1";

            var service = this.GetRequiredService<IEvenementService>();
            await service.SaveAsync(new FietsTocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByTitel(fietsTochtNaam);

            Assert.AreEqual(fietsTocht?.Titel, fietsTochtNaam);
        }

        [Test]
        public async Task GetByIdTest()
        {
            const string fietsTochtNaam = "Fietstocht2";

            var service = this.GetRequiredService<IEvenementService>();
            await service.SaveAsync(new FietsTocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByTitel(fietsTochtNaam);

            Assert.IsNotNull(fietsTocht);
            if (fietsTocht != null)
            {
                var fietsTochtById = service.GetById(fietsTocht.Id);
                Assert.IsNotNull(fietsTochtById);
                // not really necessary but to avoid warnings
                if (fietsTochtById != null)
                {
                    Assert.AreEqual(fietsTochtById.Titel, fietsTochtNaam);
                }
            }
        }

        [Test]
        public async Task AddWithExistingName()
        {
            const string fietsTochtNaam = "Fietstocht3";

            var service = this.GetRequiredService<IEvenementService>();
            await service.SaveAsync(new FietsTocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var errorCode = await service.SaveAsync(new FietsTocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2023, 1, 1) });

            Assert.AreEqual(errorCode, ErrorCodeEnum.Conflict);
        }

        [Test]
        public async Task UpdateEvenement()
        {
            const string fietsTochtNaam = "Fietstocht4";
            const string fietsTochtUpdatedNaam = "Fietstocht4.1";

            var service = this.GetRequiredService<IEvenementService>();
            var fietsTocht = new FietsTocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);

            fietsTocht.Titel = fietsTochtUpdatedNaam;
            await service.SaveAsync(fietsTocht);

            var fietsTochtById = service.GetById(fietsTocht.Id);
            Assert.IsNotNull(fietsTochtById);

            // not really necessary but to avoid warnings
            if (fietsTochtById != null)
            {
                Assert.AreEqual(fietsTochtById.Titel, fietsTochtUpdatedNaam);
            }
        }

        [Test]
        public async Task GetByProjectWithMatchingId()
        {
            const int projectId = 42;

            var service = this.GetRequiredService<IEvenementService>();
            var fietsTocht = new FietsTocht() { KentaaProjectId = projectId, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);

            var bihzProject = new BihzProject() { Id = 1, ProjectId = projectId };

            var fietsTochtByProject = service.GetByProject(bihzProject);
            Assert.IsNotNull(fietsTochtByProject);
        }

        [Test]
        public async Task GetByProjectWithMatchingTitelTest()
        {
            const string projectTitel = "Hitchhikers Galactic Cycling Tour";

            var service = this.GetRequiredService<IEvenementService>();
            var fietsTocht = new FietsTocht() { Titel = projectTitel, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);

            var bihzProject = new BihzProject() { Id = 1, Titel = projectTitel };

            var fietsTochtByProject = service.GetByProject(bihzProject);
            Assert.IsNotNull(fietsTochtByProject);
        }

        [Test]
        public async Task GetByProjectWithMatchingIdAndTitelTest()
        {
            const int projectId = 42;
            const string projectTitel = "Hitchhikers Galactic Cycling Tour";

            var service = this.GetRequiredService<IEvenementService>();
            var fietsTocht = new FietsTocht() { Titel = projectTitel, KentaaProjectId = projectId, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);

            var bihzProject = new BihzProject() { Id = 1, ProjectId = projectId, Titel = projectTitel };

            var fietsTochtByProject = service.GetByProject(bihzProject);
            Assert.IsNotNull(fietsTochtByProject);
        }

        [Test]
        public async Task GetAll()
        {
            var service = this.GetRequiredService<IEvenementService>();

            var strArray = new string[] { "aap", "noot", "mies" };
            foreach (var name in strArray)
            {
                var fietsTocht = new FietsTocht() { Titel = name, GeplandeDatum = new DateTime(2022, 1, 1) };
                await service.SaveAsync(fietsTocht);
            }

            strArray = new string[] { "wim", "zus", "jet" };
            foreach (var name in strArray)
            {
                var golfDag = new GolfDag() { Titel = name, GeplandeDatum = new DateTime(2022, 1, 1) };
                await service.SaveAsync(golfDag);
            }

            var fietsTochten = service.GetAll<FietsTocht>();
            var golfDagen = service.GetAll<GolfDag>();

            Assert.AreEqual(3, fietsTochten?.ToList().Count);
            Assert.AreEqual(3, golfDagen?.ToList().Count);
        }

        [Test]
        public async Task AddPersoon()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);
            await service.AddDeelnemerAsync(fietsTocht, new Persoon() { EmailAdres = "aap@noot.com" });
            service = null;

            var service2 = this.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            var persoon = fietsTochtById?.Deelnemers.FirstOrDefault();
            var isDeelnemerVan = persoon?.FietsTochten?.FirstOrDefault(f => f.Id == fietsTocht.Id) != null;

            Assert.IsNotNull(fietsTochtById);
            Assert.AreEqual(1, fietsTochtById?.Deelnemers.Count);
            Assert.IsTrue(isDeelnemerVan);
        }

        [Test]
        public async Task DeletePersoon()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);
            await service.AddDeelnemerAsync(fietsTocht, new Persoon() { EmailAdres = "aap@noot.com" });
            
            var service2 = this.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            var persoon = fietsTochtById?.Deelnemers.FirstOrDefault();
            Assert.IsNotNull(fietsTochtById);
            Assert.IsNotNull(persoon);

            // not really necessary but to avoid warnings
            if (fietsTochtById != null && persoon != null)
            {
                // try to delete an exisitng deelnemer from the evenement
                var result = await service2.DeleteDeelnemerAsync(fietsTochtById, persoon);
                Assert.AreEqual(ErrorCodeEnum.Ok, result);
                Assert.AreEqual(0, fietsTochtById.Deelnemers?.Count);

                // try to delete a non exisitng deelnemer from the evenement
                result = await service2.DeleteDeelnemerAsync(fietsTochtById, persoon);
                Assert.AreEqual(ErrorCodeEnum.Ok, result);
            }
        }

        [Test]
        public async Task DeletePersoonById()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IEvenementService>();

            var fietsTocht = new FietsTocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);
            var persoon = new Persoon() { EmailAdres = "aap@noot.com" };
            await service.AddDeelnemerAsync(fietsTocht, persoon);

            var service2 = this.GetRequiredService<IEvenementService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            Assert.IsNotNull(fietsTochtById);

            // not really necessary but to avoid warnings
            if (fietsTochtById != null)
            {
                // try to delete an exisitng deelnemer from the evenement
                var result = await service2.DeleteDeelnemerAsync(fietsTochtById, persoon.Id);
                Assert.AreEqual(ErrorCodeEnum.Ok, result);
                Assert.AreEqual(0, fietsTochtById.Deelnemers?.Count);

                // try to delete a non exisitng deelnemer from the evenement
                result = await service2.DeleteDeelnemerAsync(fietsTochtById, persoon);
                Assert.AreEqual(ErrorCodeEnum.Ok, result);
            }
        }
    }
}
