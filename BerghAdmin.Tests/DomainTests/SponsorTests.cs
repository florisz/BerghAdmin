using BerghAdmin.Data;
using BerghAdmin.Services;
using BerghAdmin.Services.Sponsoren;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BerghAdmin.Tests.DomainTests
{
    [TestFixture]
    public class SponsorTests : DatabaseTestSetup
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            services
                .AddScoped<ISponsorService, SponsorService>()
                .AddScoped<IPersoonService, PersoonService>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }

        [SetUp]
        public void SetupSponsorTests()
        {
        }

        [Test]
        public async Task TestSaveAndReadAmbassadeurSimple()
        {
            const string naamAmbassadeur = "Ambassadeur1";

            var ambassadeur = new Ambassadeur
            {
                Id = 0,
                Naam = naamAmbassadeur
            };
            var service = this.GetRequiredService<ISponsorService>();
            await service.SaveAsync<Ambassadeur>(ambassadeur);
            var ambassadeurRead = service.GetByNaam<Ambassadeur>(naamAmbassadeur) as Ambassadeur;

            Assert.IsNotNull(ambassadeurRead);
            Assert.AreEqual(naamAmbassadeur, ambassadeurRead!.Naam);
        }

        [Test]
        public async Task TestSaveAndReadAmbassadeurAllFields()
        {
            const string naamAmbassadeur = "Ambassadeur2";

            var ambassadeur = new Ambassadeur
            {
                Id = 0,
                Adres = "Adres",
                EmailAdres = "Email",
                Naam = naamAmbassadeur,
                Postcode = "1234 AB",
                Plaats = "Plaats",
                Land = "Nederland",
                Telefoon = "080076543",
                Mobiel = "0612345678",
                Opmerkingen = "Opmerkingen",
                ContactPersoon = new Persoon() { EmailAdres = "aap@noot.com" },
                Compagnon = new Persoon() { EmailAdres = "mies@wim.com" },
                DebiteurNummer = 123456,
                ToegezegdBedrag = 100,
                TotaalBedrag = 200,
                DatumAangebracht = new DateTime(2021, 1, 1),
                Pakket = PakketEnum.Ambassadeur,
                Fax = "080076543"
            };
            var service = this.GetRequiredService<ISponsorService>();
            await service.SaveAsync<Ambassadeur>(ambassadeur);
            var ambassadeurRead = service.GetByNaam<Ambassadeur>(naamAmbassadeur) as Ambassadeur;

            Assert.IsNotNull(ambassadeurRead);
            Assert.AreEqual(naamAmbassadeur, ambassadeurRead!.Naam);
        }

        [Test]
        public async Task TestSaveAndReadGolfdagSponsorSimple()
        {
            const string naamGolfdagSponsor = "GolfdagSponsor1";

            var golfdagSponsor = new GolfdagSponsor
            {
                Id = 0,
                Naam = naamGolfdagSponsor
            };
            var service = this.GetRequiredService<ISponsorService>();
            await service.SaveAsync<GolfdagSponsor>(golfdagSponsor);
            var golfdagSponsorRead = service.GetByNaam<GolfdagSponsor>(naamGolfdagSponsor) as GolfdagSponsor;

            Assert.IsNotNull(golfdagSponsorRead);
            Assert.AreEqual(naamGolfdagSponsor, golfdagSponsorRead!.Naam);
            Assert.AreEqual(0, golfdagSponsorRead!.GolfdagenGesponsored.Count);
        }

        [Test]
        public async Task TestSaveAndReadGolfdagSponsorAllFields()
        {
            const string naamGolfdagSponsor = "GolfdagSponsor2";

            var golfdagSponsor = new GolfdagSponsor
            {
                Id = 0,
                Adres = "Adres",
                EmailAdres = "Email",
                Naam = naamGolfdagSponsor,
                Postcode = "1234 AB",
                Plaats = "Plaats",
                Land = "Nederland",
                Telefoon = "080076543",
                Mobiel = "0612345678",
                Opmerkingen = "Opmerkingen",
                ContactPersoon = new Persoon() { EmailAdres = "aap@noot.com" },
                Compagnon = new Persoon() { EmailAdres = "mies@wim.com" },
                DebiteurNummer = 123456,
                GolfdagenGesponsored = new List<Golfdag>() { new Golfdag() { Titel = "Golfdag1" } }
            };
            var service = this.GetRequiredService<ISponsorService>();
            await service.SaveAsync<GolfdagSponsor>(golfdagSponsor);
            var golfdagSponsorRead = service.GetByNaam<GolfdagSponsor>(naamGolfdagSponsor) as GolfdagSponsor;

            Assert.IsNotNull(golfdagSponsorRead);
            Assert.AreEqual(naamGolfdagSponsor, golfdagSponsorRead!.Naam);
            Assert.AreEqual(1, golfdagSponsorRead!.GolfdagenGesponsored.Count);
        }

    }
}

