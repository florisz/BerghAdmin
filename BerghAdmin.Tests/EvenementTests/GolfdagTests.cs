using BerghAdmin.Data;
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
    public class GolfdagTests : DatabaseTestSetup
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            services
                .AddScoped<IGolfdagenService, GolfdagenService>()
                .AddScoped<IPersoonService, PersoonService>()
                .AddScoped<IRolService, RolService>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }

        [Test]
        public async Task GetByNameTest()
        {
            const string golfdagNaam = "Golfdag1";
            const string golfdagLocatie = "Golfbaan het Putje";

            var service = this.GetRequiredService<IGolfdagenService>();
            await service.SaveAsync(new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1), Locatie = golfdagLocatie });
            var golfdag = service.GetByTitel(golfdagNaam);

            Assert.IsNotNull(golfdag);
            Assert.AreEqual(golfdag?.Titel, golfdagNaam);
            Assert.AreEqual(golfdag?.Locatie, golfdagLocatie);
        }

        [Test]
        public async Task GetByIdTest()
        {
            const string golfdagNaam = "Golfdag2";

            var service = this.GetRequiredService<IGolfdagenService>();
            await service.SaveAsync(new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1)});
            
            // get by titel to get the id
            var golfdag = service.GetByTitel(golfdagNaam);
            Assert.IsNotNull(golfdag);
            var golfdagById = service.GetById(golfdag!.Id);
            Assert.IsNotNull(golfdagById);
            Assert.AreEqual(golfdagById!.Titel, golfdagNaam);
    }

        [Test]
        public async Task AddWithExistingName()
        {
            const string golfdagNaam = "Golfdag3";

            var service = this.GetRequiredService<IGolfdagenService>();
            await service.SaveAsync(new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1)});
            var errorCode = await service.SaveAsync(new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2023, 1, 1) });

            Assert.AreEqual(errorCode, ErrorCodeEnum.Conflict);
        }

        [Test]
        public async Task UpdateGolfdag()
        {
            const string golfdagNaam = "Golfdag4";
            const string golfdagUpdatedNaam = "Golfdag4.1";

            var service = this.GetRequiredService<IGolfdagenService>();
            var golfdag = new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(golfdag);

            golfdag.Titel = golfdagUpdatedNaam;
            await service.SaveAsync(golfdag);

            var golfdagById = service.GetById(golfdag.Id);
            Assert.IsNotNull(golfdagById);
            Assert.AreEqual(golfdagById!.Titel, golfdagUpdatedNaam);
        }

        [Test]
        public async Task GetAll()
        {
            var service = this.GetRequiredService<IGolfdagenService>();

            var strArray = new string[] { "aap", "noot", "mies" };
            foreach (var name in strArray)
            {
                var golfdag = new Golfdag() { Titel = name, GeplandeDatum = new DateTime(2022, 1, 1) };
                await service.SaveAsync(golfdag);
            }

            var golfdagen = service.GetAll();

            Assert.AreEqual(3, golfdagen?.ToList().Count);
        }

        [Test]
        public async Task AddDeelnemer()
        {
            const string golfdagNaam = "Golfdag5";

            var service = this.GetRequiredService<IGolfdagenService>();

            var golfdag = new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(golfdag);
            await service.AddDeelnemerAsync(golfdag, new Persoon() { EmailAdres = "aap@noot.com" });
            service = null;

            var service2 = this.GetRequiredService<IGolfdagenService>();
            var golfdagById = service2.GetById(golfdag.Id);
            var persoon = golfdagById?.Deelnemers.FirstOrDefault();
            var isDeelnemerVan = persoon?.Golfdagen?.FirstOrDefault(f => f.Id == golfdag.Id) != null;

            Assert.IsNotNull(golfdagById);
            Assert.AreEqual(1, golfdagById?.Deelnemers.Count);
            Assert.IsTrue(isDeelnemerVan);
        }

        [Test]
        public async Task DeleteDeelnemer()
        {
            const string golfdagNaam = "Golfdag6";

            var service = this.GetRequiredService<IGolfdagenService>();

            var golfdag = new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(golfdag);
            await service.AddDeelnemerAsync(golfdag, new Persoon() { EmailAdres = "aap@noot.com" });
            
            var service2 = this.GetRequiredService<IGolfdagenService>();
            var golfdagById = service2.GetById(golfdag.Id);
            var persoon = golfdagById?.Deelnemers.FirstOrDefault();
            Assert.IsNotNull(golfdagById);
            Assert.IsNotNull(persoon);

            // try to delete an exisitng deelnemer from the golfdag
            var result = await service2.DeleteDeelnemerAsync(golfdagById!, persoon!);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, golfdagById!.Deelnemers?.Count);

            // try to delete a non exisitng deelnemer from the golfdag
            result = await service2.DeleteDeelnemerAsync(golfdagById, persoon!);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }

        [Test]
        public async Task DeleteDeelnemerById()
        {
            const string golfdagNaam = "Golfdag4";

            var service = this.GetRequiredService<IGolfdagenService>();

            var golfdag = new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(golfdag);
            var persoon = new Persoon() { EmailAdres = "aap@noot.com" };
            await service.AddDeelnemerAsync(golfdag, persoon);

            // try to delete an exisitng deelnemer from the golfdag
            var result = await service.DeleteDeelnemerAsync(golfdag!, persoon.Id);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, golfdag!.Deelnemers?.Count);

            // try to delete a non existing deelnemer from the golfdag
            result = await service.DeleteDeelnemerAsync(golfdag!, persoon);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }

        [Test]
        public async Task AddSponsor()
        {
            const string golfdagNaam = "Golfdag5";
            const string sponsorNaam = "De gulle gever";

            var service = this.GetRequiredService<IGolfdagenService>();

            var contactPersoon = new Persoon() { EmailAdres = "aap@noot.com" };
            var golfdag = new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(golfdag);
            await service.AddSponsorAsync(golfdag, new GolfdagSponsor() { Naam = sponsorNaam,  
                                                                            DebiteurNummer = "123",
                                                                            EmailAdres = "sponsor@bedrijf.nl", 
                                                                            ContactPersoon1 = contactPersoon });
            var golfdagById = service.GetById(golfdag.Id);
            var sponsor = golfdag?.Sponsoren.FirstOrDefault();
            var isSponsorVan = sponsor?.GolfdagenGesponsored.FirstOrDefault(f => f.Id == golfdagById!.Id) != null;
            Assert.IsNotNull(sponsor);
            Assert.AreEqual(sponsor!.Naam, sponsorNaam);
            Assert.IsNotNull(golfdagById);
            Assert.AreEqual(1, golfdagById?.Sponsoren.Count);
        }

        [Test]
        public async Task DeleteSponsor()
        {
            const string golfdagNaam = "Golfdag6";
            const string sponsorNaam = "De gulle gever";

            var service = this.GetRequiredService<IGolfdagenService>();

            var golfdag = new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(golfdag);
            await service.AddSponsorAsync(golfdag, new GolfdagSponsor() { Naam = sponsorNaam, DebiteurNummer = "123", EmailAdres = "aap@noot.com" });

            var sponsor = golfdag.Sponsoren.FirstOrDefault();
            Assert.IsNotNull(sponsor);

            // try to delete an existing deelnemer from the golfdag
            var result = await service.DeleteSponsorAsync(golfdag, sponsor!);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, golfdag.Deelnemers?.Count);

            // try to delete a non exisitng deelnemer from the golfdag
            result = await service.DeleteSponsorAsync(golfdag, sponsor!);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }

        [Test]
        public async Task DeleteSponsorById()
        {
            const string golfdagNaam = "Golfdag4";

            var service = this.GetRequiredService<IGolfdagenService>();

            var golfdag = new Golfdag() { Titel = golfdagNaam, GeplandeDatum = new DateTime(2022, 1, 1) };
            await service.SaveAsync(golfdag);
            var persoon = new Persoon() { EmailAdres = "aap@noot.com" };
            await service.AddDeelnemerAsync(golfdag, persoon);

            // try to delete an exisitng deelnemer from the golfdag
            var result = await service.DeleteDeelnemerAsync(golfdag!, persoon.Id);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
            Assert.AreEqual(0, golfdag!.Deelnemers?.Count);

            // try to delete a non existing deelnemer from the golfdag
            result = await service.DeleteDeelnemerAsync(golfdag!, persoon);
            Assert.AreEqual(ErrorCodeEnum.Ok, result);
        }
    }
}
