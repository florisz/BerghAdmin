using BerghAdmin.Data;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;
using BerghAdmin.Services;
using BerghAdmin.Services.Evenementen;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BerghAdmin.Tests.EvenementenTests
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
            const string fietstochtNaam = "Fietstocht1";

            var service = this.GetRequiredService<IFietstochtenService>();
            await service.SaveAsync(new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var fietstocht = service.GetByTitel(fietstochtNaam);

            Assert.AreEqual(fietstocht?.Titel, fietstochtNaam);
        }

        [Test]
        public async Task GetByIdTest()
        {
            const string fietstochtNaam = "Fietstocht2";

            var service = this.GetRequiredService<IFietstochtenService>();
            await service.SaveAsync(new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var fietstocht = service.GetByTitel(fietstochtNaam);

            Assert.IsNotNull(fietstocht);
            if (fietstocht != null)
            {
                var fietstochtById = service.GetById(fietstocht.Id);
                Assert.IsNotNull(fietstochtById);
                Assert.AreEqual(fietstochtById!.Titel, fietstochtNaam);
            }
        }

        [Test]
        public async Task AddWithExistingName()
        {
            const string fietstochtNaam = "Fietstocht3";

            var service = this.GetRequiredService<IFietstochtenService>();
            await service.SaveAsync(new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var errorCode = await service.SaveAsync(new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2023, 1, 1) });

            Assert.AreEqual(errorCode, ErrorCodeEnum.Conflict);
        }

        [Test]
        public async Task UpdateFietstocht()
        {
            const string fietstochtNaam = "Fietstocht4";
            const string fietstochtUpdatedNaam = "Fietstocht4.1";

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietstocht = new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);

            fietstocht.Titel = fietstochtUpdatedNaam;
            await service.SaveAsync(fietstocht);

            var fietstochtById = service.GetById(fietstocht.Id);
            Assert.IsNotNull(fietstochtById);
            Assert.AreEqual(fietstochtById!.Titel, fietstochtUpdatedNaam);
        }

        [Test]
        public async Task GetByProjectWithMatchingId()
        {
            const int projectId = 42;

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietstocht = new Fietstocht() { KentaaProjectId = projectId, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);

            var bihzProject = new BihzProject() { Id = 1, ProjectId = projectId };

            var fietstochtByProject = service.GetByProject(bihzProject);
            Assert.IsNotNull(fietstochtByProject);
        }

        [Test]
        public async Task GetByProjectWithMatchingTitelTest()
        {
            const string projectTitel = "Hitchhikers Galactic Cycling Tour";

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietstocht = new Fietstocht() { Titel = projectTitel, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);

            var bihzProject = new BihzProject() { Id = 1, Titel = projectTitel };

            var fietstochtByProject = service.GetByProject(bihzProject);
            Assert.IsNotNull(fietstochtByProject);
        }

        [Test]
        public async Task GetByProjectWithMatchingIdAndTitelTest()
        {
            const int projectId = 42;
            const string projectTitel = "Hitchhikers Galactic Cycling Tour";

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietstocht = new Fietstocht() { Titel = projectTitel, KentaaProjectId = projectId, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);

            var bihzProject = new BihzProject() { Id = 1, ProjectId = projectId, Titel = projectTitel };

            var fietstochtByProject = service.GetByProject(bihzProject);
            Assert.IsNotNull(fietstochtByProject);
        }

        [Test]
        public async Task GetAll()
        {
            var service = this.GetRequiredService<IFietstochtenService>();

            var strArray = new string[] { "aap", "noot", "mies" };
            foreach (var name in strArray)
            {
                var fietstocht = new Fietstocht() { Titel = name, GeplandeDatum = new DateTime(2022, 1, 1) };
                await service.SaveAsync(fietstocht);
            }

            var fietstochten = service.GetAll();

            Assert.AreEqual(3, fietstochten?.ToList().Count);
        }

        [Test]
        public async Task AddPersoon()
        {
            const string fietstochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IFietstochtenService>();

            var fietstocht = new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);
            await service.AddDeelnemerAsync(fietstocht, new Persoon() { EmailAdres = "aap@noot.com" });
            service = null;

            var service2 = this.GetRequiredService<IFietstochtenService>();
            var fietstochtById = service2.GetById(fietstocht.Id);
            var persoon = fietstochtById?.Deelnemers.FirstOrDefault();
            var isDeelnemerVan = persoon?.Fietstochten?.FirstOrDefault(f => f.Id == fietstocht.Id) != null;

            Assert.IsNotNull(fietstochtById);
            Assert.AreEqual(1, fietstochtById?.Deelnemers.Count);
            Assert.IsTrue(isDeelnemerVan);
        }

        [Test]
        public async Task DeletePersoon()
        {
            const string fietstochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IFietstochtenService>();

            var fietstocht = new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);
            await service.AddDeelnemerAsync(fietstocht, new Persoon() { EmailAdres = "aap@noot.com" });
            
            var service2 = this.GetRequiredService<IFietstochtenService>();
            var fietstochtById = service2.GetById(fietstocht.Id);
            var persoon = fietstochtById?.Deelnemers.FirstOrDefault();
            Assert.IsNotNull(fietstochtById);
            Assert.IsNotNull(persoon);

            // try to delete an exisitng deelnemer from the fietstocht
            var result = await service2.DeleteDeelnemerAsync(fietstochtById!, persoon!);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, fietstochtById!.Deelnemers?.Count);

            // try to delete a non exisitng deelnemer from the fietstocht
            result = await service2.DeleteDeelnemerAsync(fietstochtById, persoon!);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }

        [Test]
        public async Task DeletePersoonById()
        {
            const string fietstochtNaam = "Fietstocht4";

            var service = this.GetRequiredService<IFietstochtenService>();

            var fietstocht = new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);
            var persoon = new Persoon() { EmailAdres = "aap@noot.com" };
            await service.AddDeelnemerAsync(fietstocht, persoon);

            var service2 = this.GetRequiredService<IFietstochtenService>();
            var fietstochtById = service2.GetById(fietstocht.Id);
            Assert.IsNotNull(fietstochtById);

            // try to delete an exisitng deelnemer from the fietstocht
            var result = await service2.DeleteDeelnemerAsync(fietstochtById!, persoon.Id);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, fietstochtById!.Deelnemers?.Count);

            // try to delete a non exisitng deelnemer from the fietstocht
            result = await service2.DeleteDeelnemerAsync(fietstochtById!, persoon);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }
    }
}
