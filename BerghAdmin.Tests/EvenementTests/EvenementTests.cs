using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerghAdmin.Tests.EvenementTests
{
    [TestFixture]
    public class EvenementTests
    {
        private readonly ServiceProvider _serviceProvider;
        private ApplicationDbContext _applicationDbContext;

        public EvenementTests()
        {
            var connection = InMemoryDatabaseHelper.GetSqliteInMemoryConnection();
            var services = new ServiceCollection();
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<ApplicationDbContext>(builder =>
                {
                    builder.UseSqlite(connection);
                }, ServiceLifetime.Singleton, ServiceLifetime.Singleton)
                .AddScoped<IEvenementService, EvenementService>();
            
            _serviceProvider = services.BuildServiceProvider();
        }

        [SetUp]
        public void SetupTests()
        {
            var scope = _serviceProvider.CreateScope();
            _applicationDbContext = scope?.ServiceProvider.GetService<ApplicationDbContext>();

            _applicationDbContext?.Database.OpenConnection();
            _applicationDbContext?.Database.EnsureCreated();
        }

        [Test]
        public void GetByNameTest()
        {
            const string fietsTochtNaam = "Fietstocht1";

            var service = _serviceProvider.GetRequiredService<IEvenementService>();
            service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByName(fietsTochtNaam);

            Assert.AreEqual(fietsTocht.Naam, fietsTochtNaam);
            Assert.Pass();
        }

        [Test]
        public void GetByIdTest()
        {
            const string fietsTochtNaam = "Fietstocht2";

            var service = _serviceProvider.GetRequiredService<IEvenementService>();
            service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var fietsTocht = service.GetByName(fietsTochtNaam);

            Assert.IsNotNull(fietsTocht);

            var fietsTochtById = service.GetById(fietsTocht.Id);

            Assert.AreEqual(fietsTochtById.Naam, fietsTochtNaam);
            Assert.Pass();
        }

        [Test]
        public void AddWithExistingName()
        {
            const string fietsTochtNaam = "Fietstocht3";

            var service = _serviceProvider.GetRequiredService<IEvenementService>();
            service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2022, 1, 1) });
            var errorCode = service.SaveEvenement(new FietsTocht() { Naam = fietsTochtNaam, GeplandJaar = new DateTime(2023, 1, 1) });

            Assert.AreEqual(errorCode, ErrorCodeEnum.Conflict);
            Assert.Pass();
        }

    }
}
