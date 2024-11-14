using BerghAdmin.Data;
using BerghAdmin.Services.DateTimeProvider;
using BerghAdmin.Services.Documenten;
using BerghAdmin.Services.Facturen;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BerghAdmin.Tests.FactuurTests
{
    [TestFixture]
    public class FactuurTests : DatabaseTestSetup
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            services
                .AddScoped<IFactuurService, FactuurService>()
                .AddScoped<IDocumentService, DocumentService>()
                .AddScoped<IDocumentMergeService, DocumentMergeService>()
                .AddScoped<IPdfConverter, PdfConverter>()
                .AddScoped<IDateTimeProvider, TestDateTimeProvider>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }

        [Test]
        public async Task GetNewFactuurTest()
        {
            var factuurService = this.GetRequiredService<IFactuurService>();
            Ambassadeur ambassadeur = new Ambassadeur() { DebiteurNummer = "1", Naam = "aap" };
            var factuur = await factuurService.GetNewFactuurAsync(ambassadeur);
            // TO DO: solve whatif twee facturen op precies hetzelfde moment hetzelfde nummer krijgen
            var nummer = factuur!.Nummer;
            factuur = await factuurService.GetFactuurAsync(nummer);

            Assert.IsNotNull(factuur);
            Assert.AreEqual(factuur!.Nummer, nummer);
        }

        [Test]
        public async Task GetNewFactuurWhichExists()
        {
            // TO DO: this is a multi-threaded test, which is not supported by the current test setup
            // if needed implement this one later

            Assert.Pass();
        }

        [Test]
        public async Task GetNewFactuurWithSpecifiedDate()
        {
            var factuurService = this.GetRequiredService<IFactuurService>();
            var dateTimeProvider = this.GetRequiredService<IDateTimeProvider>();

            var datum = dateTimeProvider.Now;
            var ambassadeur = new Ambassadeur() { DebiteurNummer = "1", Naam = "aap" };
            var factuur = await factuurService.GetNewFactuurAsync(datum, ambassadeur);

            Assert.IsNotNull(factuur);
            Assert.AreEqual(factuur!.Datum.ToShortDateString(), dateTimeProvider.Now.ToShortDateString());
        }
    }
}
