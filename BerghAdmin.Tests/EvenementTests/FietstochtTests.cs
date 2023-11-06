using BerghAdmin.Data;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;
using BerghAdmin.Services;
using BerghAdmin.Services.Evenementen;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BerghAdmin.Tests.FietstochtenTests
{
    [TestFixture]
    public class FietstochtTests : DatabaseTestSetup
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            services
                .AddScoped<IFietstochtenService, FietstochtenService>()
                .AddScoped<IPersoonService, PersoonService>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }

        [Test]
        public async Task GetByNameTest()
        {
            const string fietsTochtNaam = "Fietstocht1";

            var service = this.GetRequiredService<IFietstochtenService>();
            await service.SaveAsync(new Fietstocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByTitel(fietsTochtNaam);

            Assert.AreEqual(fietsTocht?.Titel, fietsTochtNaam);
        }

        [Test]
        public async Task GetByIdTest()
        {
            const string fietsTochtNaam = "Fietstocht2";

            var service = this.GetRequiredService<IFietstochtenService>();
            await service.SaveAsync(new Fietstocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByTitel(fietsTochtNaam);

            Assert.IsNotNull(fietsTocht);
            if (fietsTocht != null)
            {
                var fietsTochtById = service.GetById(fietsTocht.Id);
                Assert.IsNotNull(fietsTochtById);
                Assert.AreEqual(fietsTochtById!.Titel, fietsTochtNaam);
            }
        }

        [Test]
        public async Task AddWithExistingName()
        {
            const string fietsTochtNaam = "Fietstocht3";

            var service = this.GetRequiredService<IFietstochtenService>();
            await service.SaveAsync(new Fietstocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var errorCode = await service.SaveAsync(new Fietstocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2023, 1, 1) });

            Assert.AreEqual(errorCode, ErrorCodeEnum.Conflict);
        }

        [Test]
        public async Task UpdateFietstocht()
        {
            const string fietsTochtNaam = "Fietstocht4";
            const string fietsTochtUpdatedNaam = "Fietstocht4.1";

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietsTocht = new Fietstocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);

            fietsTocht.Titel = fietsTochtUpdatedNaam;
            await service.SaveAsync(fietsTocht);

            var fietsTochtById = service.GetById(fietsTocht.Id);
            Assert.IsNotNull(fietsTochtById);
            Assert.AreEqual(fietsTochtById!.Titel, fietsTochtUpdatedNaam);
        }

        [Test]
        public async Task GetByProjectWithMatchingId()
        {
            const int projectId = 42;

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietsTocht = new Fietstocht() { KentaaProjectId = projectId, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);

            var bihzProject = new BihzProject() { Id = 1, ProjectId = projectId };

            var fietsTochtByProject = service.GetByProject(bihzProject);
            Assert.IsNotNull(fietsTochtByProject);
        }

        [Test]
        public async Task GetByProjectWithMatchingTitelTest()
        {
            const string projectTitel = "Hitchhikers Galactic Cycling Tour";

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietsTocht = new Fietstocht() { Titel = projectTitel, GeplandeDatum = new DateTime(2022, 1, 1) };
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

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietsTocht = new Fietstocht() { Titel = projectTitel, KentaaProjectId = projectId, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);

            var bihzProject = new BihzProject() { Id = 1, ProjectId = projectId, Titel = projectTitel };

            var fietsTochtByProject = service.GetByProject(bihzProject);
            Assert.IsNotNull(fietsTochtByProject);
        }

        [Test]
        public async Task GetAll()
        {
            var service = this.GetRequiredService<IFietstochtenService>();

            var strArray = new string[] { "aap", "noot", "mies" };
            foreach (var name in strArray)
            {
                var fietsTocht = new Fietstocht() { Titel = name, GeplandeDatum = new DateTime(2022, 1, 1) };
                await service.SaveAsync(fietsTocht);
            }

            //strArray = new string[] { "wim", "zus", "jet" };
            //foreach (var name in strArray)
            //{
            //    var golfDag = new Golfdag() { Titel = name, GeplandeDatum = new DateTime(2022, 1, 1) };
            //    await service.SaveAsync(golfDag);
            //}

            var fietsTochten = service.GetAll();
            //var golfDagen = service.GetAll<Golfdag>();

            Assert.AreEqual(3, fietsTochten?.ToList().Count);
            //Assert.AreEqual(3, golfDagen?.ToList().Count);
        }

        [Test]
        public async Task AddPersoon()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IFietstochtenService>();

            var fietsTocht = new Fietstocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);
            await service.AddDeelnemerAsync(fietsTocht, new Persoon() { EmailAdres = "aap@noot.com" });
            service = null;

            var service2 = this.GetRequiredService<IFietstochtenService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            var persoon = fietsTochtById?.Deelnemers.FirstOrDefault();
            var isDeelnemerVan = persoon?.Fietstochten?.FirstOrDefault(f => f.Id == fietsTocht.Id) != null;

            Assert.IsNotNull(fietsTochtById);
            Assert.AreEqual(1, fietsTochtById?.Deelnemers.Count);
            Assert.IsTrue(isDeelnemerVan);
        }

        [Test]
        public async Task DeletePersoon()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IFietstochtenService>();

            var fietsTocht = new Fietstocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);
            await service.AddDeelnemerAsync(fietsTocht, new Persoon() { EmailAdres = "aap@noot.com" });
            
            var service2 = this.GetRequiredService<IFietstochtenService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            var persoon = fietsTochtById?.Deelnemers.FirstOrDefault();
            Assert.IsNotNull(fietsTochtById);
            Assert.IsNotNull(persoon);

            // try to delete an exisitng deelnemer from the fietstocht
            var result = await service2.DeleteDeelnemerAsync(fietsTochtById!, persoon!);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, fietsTochtById!.Deelnemers?.Count);

            // try to delete a non exisitng deelnemer from the fietstocht
            result = await service2.DeleteDeelnemerAsync(fietsTochtById, persoon!);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }

        [Test]
        public async Task DeletePersoonById()
        {
            const string fietsTochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IFietstochtenService>();

            var fietsTocht = new Fietstocht() { Titel = fietsTochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietsTocht);
            var persoon = new Persoon() { EmailAdres = "aap@noot.com" };
            await service.AddDeelnemerAsync(fietsTocht, persoon);

            var service2 = this.GetRequiredService<IFietstochtenService>();
            var fietsTochtById = service2.GetById(fietsTocht.Id);
            Assert.IsNotNull(fietsTochtById);

            // try to delete an exisitng deelnemer from the fietstocht
            var result = await service2.DeleteDeelnemerAsync(fietsTochtById!, persoon.Id);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, fietsTochtById!.Deelnemers?.Count);

            // try to delete a non exisitng deelnemer from the fietstocht
            result = await service2.DeleteDeelnemerAsync(fietsTochtById!, persoon);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }
    }
}
