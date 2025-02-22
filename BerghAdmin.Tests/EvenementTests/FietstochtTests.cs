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
            var fietstocht = await service.GetByTitel(fietstochtNaam);

            Assert.That(fietstocht?.Titel == fietstochtNaam);
        }

        [Test]
        public async Task GetByIdTest()
        {
            const string fietstochtNaam = "Fietstocht2";

            var service = this.GetRequiredService<IFietstochtenService>();
            await service.SaveAsync(new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var fietstocht = await service.GetByTitel(fietstochtNaam);

            Assert.That(fietstocht, !Is.EqualTo(null));
            if (fietstocht != null)
            {
                var fietstochtById = await service.GetById(fietstocht.Id);
                Assert.That(fietstochtById, !Is.EqualTo(null));
                Assert.That(fietstochtById!.Titel == fietstochtNaam);
            }
        }

        [Test]
        public async Task AddWithExistingName()
        {
            const string fietstochtNaam = "Fietstocht3";

            var service = this.GetRequiredService<IFietstochtenService>();
            await service.SaveAsync(new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2022, 1, 1) });
            var errorCode = await service.SaveAsync(new Fietstocht() { Titel = fietstochtNaam, GeplandeDatum = new DateTime(2023, 1, 1) });

            Assert.That(errorCode == ErrorCodeEnum.Conflict);
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

            var fietstochtById = await service.GetById(fietstocht.Id);
            Assert.That(fietstochtById, !Is.EqualTo(null));
            Assert.That(fietstochtById!.Titel == fietstochtUpdatedNaam);
        }

        [Test]
        public async Task GetByProjectWithMatchingId()
        {
            const int projectId = 42;

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietstocht = new Fietstocht() { KentaaProjectId = projectId, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);

            var bihzProject = new BihzProject() { Id = 1, ProjectId = projectId };

            var fietstochtByProject = await service.GetByProject(bihzProject);
            Assert.That(fietstochtByProject, !Is.EqualTo(null));
        }

        [Test]
        public async Task GetByProjectWithMatchingTitelTest()
        {
            const string projectTitel = "Hitchhikers Galactic Cycling Tour";

            var service = this.GetRequiredService<IFietstochtenService>();
            var fietstocht = new Fietstocht() { Titel = projectTitel, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(fietstocht);

            var bihzProject = new BihzProject() { Id = 1, Titel = projectTitel };

            var fietstochtByProject = await service.GetByProject(bihzProject);
            Assert.That(fietstochtByProject, !Is.EqualTo(null));
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

            var fietstochtByProject = await service.GetByProject(bihzProject);
            Assert.That(fietstochtByProject, !Is.EqualTo(null));
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

            var fietstochten = await service.GetAll();

            Assert.That(3 == fietstochten?.ToList().Count);
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
            var fietstochtById = await service2.GetById(fietstocht.Id);
            var persoon = fietstochtById?.Deelnemers.FirstOrDefault();
            var isDeelnemerVan = persoon?.Fietstochten?.FirstOrDefault(f => f.Id == fietstocht.Id) != null;

            Assert.That(fietstochtById, !Is.EqualTo(null));
            Assert.That(1 == fietstochtById?.Deelnemers.Count);
            Assert.That(isDeelnemerVan == true);
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
            var fietstochtById = await service2.GetById(fietstocht.Id);
            var persoon = fietstochtById?.Deelnemers.FirstOrDefault();
            Assert.That(fietstochtById, !Is.EqualTo(null));
            Assert.That(persoon, !Is.EqualTo(null));

            // try to delete an exisitng deelnemer from the fietstocht
            var result = await service2.DeleteDeelnemerAsync(fietstochtById!, persoon!);
            Assert.That(ErrorCodeEnum.Ok == result);
            Assert.That(0 == fietstochtById!.Deelnemers?.Count);

            // try to delete a non exisitng deelnemer from the fietstocht
            result = await service2.DeleteDeelnemerAsync(fietstochtById, persoon!);
            Assert.That(ErrorCodeEnum.Ok == result);
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
            var fietstochtById = await  service2.GetById(fietstocht.Id);
            Assert.That(fietstochtById, !Is.EqualTo(null));

            // try to delete an exisitng deelnemer from the fietstocht
            var result = await service2.DeleteDeelnemerAsync(fietstochtById!, persoon);
            Assert.That(ErrorCodeEnum.Ok == result);
            Assert.That(0 == fietstochtById!.Deelnemers?.Count);

            // try to delete a non exisitng deelnemer from the fietstocht
            result = await service2.DeleteDeelnemerAsync(fietstochtById!, persoon);
            Assert.That(ErrorCodeEnum.Ok == result);
        }
    }
}
