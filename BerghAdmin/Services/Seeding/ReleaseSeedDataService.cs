using BerghAdmin.DbContexts;
using BerghAdmin.Services.Evenementen;

namespace BerghAdmin.Services.Seeding;

public class ReleaseSeedDataService : ISeedDataService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IRolService _rolService;
    private readonly IEvenementService _evenementService;

    public ReleaseSeedDataService(
        ApplicationDbContext dbContext,
        IRolService rolService,
        IEvenementService evenementService)
    {
        this._dbContext = dbContext;
        this._rolService = rolService;
        this._evenementService = evenementService;
    }

    public async Task SeedInitialData()
    {
        if (SeedHelper.DatabaseHasData(_rolService))
        {
            return;
        }

        await SeedHelper.InsertRollen(_dbContext);

        await InsertEvenementen();
    }
    private async Task InsertEvenementen()
    {
        var fietstocht = new FietsTocht()
        {
            Id = 0,
            GeplandeDatum = new DateTime(2015, 5, 9),
            Titel = "Klaver Vier Tocht 2015"
        };
        await this._evenementService.Save(fietstocht);
        fietstocht = new FietsTocht()
        {
            Id = 0,
            GeplandeDatum = new DateTime(2019, 5, 24),
            Titel = "Bergh-Leipzig-Bergh 2019"
        };
        await this._evenementService.Save(fietstocht);
        fietstocht = new FietsTocht()
        {
            Id = 0,
            GeplandeDatum = new DateTime(2023, 5, 3),
            Titel = "Fietstocht 2023"
        };
        await this._evenementService.Save(fietstocht);
    }
}
