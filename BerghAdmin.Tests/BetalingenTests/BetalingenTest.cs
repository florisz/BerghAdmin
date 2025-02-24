﻿using BerghAdmin.ApplicationServices.KentaaInterface;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Betalingen;
using BerghAdmin.Services.Bihz;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Seeding;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace BerghAdmin.Tests.BetalingenTests
{
    [TestFixture]
    public class BetalingenTests : DatabaseTestSetup
    {
        private IBetalingenService? _betalingenService;
        private IBetalingenImporterService? _betalingenImporterService;

        protected override void RegisterServices(ServiceCollection services)
        {
            var databaseConfiguration = new ConfigurationBuilder()
                .AddUserSecrets<DatabaseConfiguration>()
                .Build();

            services
                .AddOptions()
                .AddScoped<IBihzActieService, BihzActieService>()
                .AddScoped<IBihzDonatieService, BihzDonatieService>()
                .AddScoped<IBihzProjectService, BihzProjectService>()
                .AddScoped<IBihzUserService, BihzUserService>()
                .AddScoped<IFietstochtenService, FietstochtenService>()
                .AddScoped<IRolService, RolService>()
                .AddScoped<IPersoonService, PersoonService>()
                .AddScoped<IFietstochtenService, FietstochtenService>()
                .AddScoped<IDonatieService, DonatieService>()
                .AddHttpClient()
                .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>()

                .Configure<SeedSettings>(databaseConfiguration.GetSection("Seeding"))
                .AddScoped<ISeedDataService, DebugSeedDataService>()
                .AddScoped<IBetalingenImporterService, BetalingenImporterService>()
                //.AddScoped<IBetalingenRepository, TableStorageBetalingenRepository>()
                .AddScoped<IBetalingenRepository, EFBetalingenRepository>()
                .AddScoped<IBetalingenService, BetalingenService>()
            ;

            services.AddDbContext<ApplicationDbContext>(
                    options => options.UseMySql(GetDatabaseConnectionString(databaseConfiguration), ServerVersion.Parse("5.7"), po => po.EnableRetryOnFailure()));

        }

        static string GetDatabaseConnectionString(IConfigurationRoot configuration)
        {
            var databaseConfiguration = configuration.GetSection("DatabaseConfiguration").Get<DatabaseConfiguration>();
            if (databaseConfiguration == null)
            {
                throw new ApplicationException("Secrets for Database access (connection string & password) can not be found in configuration");
            }
            return databaseConfiguration.ConnectionString ?? throw new ArgumentException("ConnectionString not specified");
        }

        [SetUp]
        public void Setup()
        {
            _betalingenService = GetRequiredService<IBetalingenService>();
            _betalingenImporterService = GetRequiredService<IBetalingenImporterService>();
        }

        [Test]
        public async Task TestImportRaboBetalingenBestand()
        {
            using FileStream fs = File.OpenRead("BetalingenTests/TestRaboBetalingenBestand.csv");
            var betalingen = _betalingenImporterService!.ImportBetalingen(fs);

            foreach (var betaling in betalingen)
            {
                if (_betalingenService!.GetByVolgnummer(betaling.Volgnummer) == null)
                {
                    await _betalingenService.SaveAsync(betaling);
                }
            }

            Assert.That(betalingen.Count >= 8, "Import must result in at least 8 betalingen");
            Assert.That(betalingen.Count == _betalingenService!.GetAll()?.Count());
        }
    }
}
