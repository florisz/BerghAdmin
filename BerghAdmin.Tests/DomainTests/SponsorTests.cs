using NUnit.Framework;
using System.Collections.Generic;
using BerghAdmin.Data;
using System.Linq;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using BerghAdmin.Services.Sponsoren;
using System.Drawing.Text;

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
        public async Task TestSaveAndReadSimple()
        {
            const string naamAmbassadeur = "Ambassadeur1";

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
            await service.SaveAsync(ambassadeur);
            var ambassadeurRead = service.GetByNaam(naamAmbassadeur);

            Assert.IsNotNull(ambassadeurRead);
            Assert.AreEqual(naamAmbassadeur, ambassadeurRead!.Naam);
        }

    }
}

