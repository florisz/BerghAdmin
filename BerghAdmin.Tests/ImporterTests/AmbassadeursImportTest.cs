using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.DateTimeProvider;
using BerghAdmin.Services.Documenten;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Import;
using BerghAdmin.Services.Seeding;
using BerghAdmin.Services.Sponsoren;
using Microsoft.Build.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BerghAdmin.Tests.ImporterTests;

[TestFixture]
public class AmbassadeursImportTest : DatabaseTestSetup
{
    private readonly IRolService _rolService;
    private readonly IFietstochtenService _fietstochtenService;
    private readonly IGolfdagenService _golfdagenService;
    private readonly IPersoonService _persoonService;
    private readonly ISponsorService _sponsorService;
    private readonly IAmbassadeurService _ambassadeurService;
    private readonly IDocumentService _documentService;

    protected override void RegisterServices(ServiceCollection services)
    {
        var databaseConfiguration = new ConfigurationBuilder()
            .AddUserSecrets<DatabaseConfiguration>()
            .Build();
        services
            .AddScoped<IImporterService, AmbassadeurImporterService>()
            .AddScoped<IRolService, RolService>()
            .AddScoped<IPersoonService, PersoonService>()
            .AddScoped<IFietstochtenService, FietstochtenService>()
            .AddScoped<IGolfdagenService, GolfdagenService>()
            .AddScoped<ISponsorService, SponsorService>()
            .AddScoped<IAmbassadeurService, AmbassadeurService>()
            .AddScoped<IDocumentService, DocumentService>()
            .AddScoped<IMagazineService, MagazineService>()
            .AddScoped<IDateTimeProvider, TestDateTimeProvider>()
            .AddScoped<ISeedDataService, ReleaseSeedDataService>()
            .Configure<SeedSettings>(databaseConfiguration.GetSection("Seeding"))
            .AddLogging(builder => builder.AddConsole());

    }

    static string GetDatabaseConnectionString(IConfigurationRoot configuration)
    {
        var databaseConfiguration = configuration.GetSection("DatabaseConfiguration").Get<DatabaseConfiguration>();
        return databaseConfiguration == null
            ? throw new ApplicationException("Secrets for Database access (connection string & password) can not be found in configuration")
            : databaseConfiguration.ConnectionString ?? throw new ArgumentException("ConnectionString not specified");
    }


    [Test]
    public async Task ImportAmbassadeursTest()
    {
        var importerService = this.GetRequiredService<IImporterService>();
        await InsertInitialData();

        var file = new FileInfo("C:\\Users\\fzwar\\OneDrive\\Documents\\Prive\\BIHZ\\ICT\\240228ImportTotaallijstAmbassadeurs.csv");
        var stream = file.OpenRead();
        await importerService.ImportDataAsync(stream);
    }

    private async Task InsertInitialData()
    {
        var seedDataService = GetRequiredService<ISeedDataService>();
        await seedDataService.SeedInitialData();
    }
}