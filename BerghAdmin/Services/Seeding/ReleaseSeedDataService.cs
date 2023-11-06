using BerghAdmin.DbContexts;
using BerghAdmin.Services.Evenementen;

namespace BerghAdmin.Services.Seeding;

public class ReleaseSeedDataService : ISeedDataService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IRolService _rolService;
    private readonly IFietstochtenService _fietstochtenService;

    public ReleaseSeedDataService(
        ApplicationDbContext dbContext,
        IRolService rolService,
        IFietstochtenService fietstochtenService)
    {
        this._dbContext = dbContext;
        this._rolService = rolService;
        this._fietstochtenService = fietstochtenService;
    }

    public async Task SeedInitialData()
    {
        if (SeedHelper.DatabaseHasData(_rolService))
        {
            return;
        }

        await SeedHelper.InsertRollen(_rolService);

        await InsertFietstochten();
    }
    private async Task InsertFietstochten()
    {
        var fietstocht = new Fietstocht()
        {
            Id = 0,
            GeplandeDatum = new DateTime(2015, 5, 9),
            Titel = "Klaver Vier Tocht 2015"
        };
        await this._fietstochtenService.SaveAsync(fietstocht);
        fietstocht = new Fietstocht()
        {
            Id = 0,
            GeplandeDatum = new DateTime(2019, 5, 24),
            Titel = "Bergh-Leipzig-Bergh 2019"
        };
        await this._fietstochtenService.SaveAsync(fietstocht);
        fietstocht = new Fietstocht()
        {
            Id = 0,
            GeplandeDatum = new DateTime(2023, 5, 3),
            Titel = "Hanzetocht 2023"
        };
        await this._fietstochtenService.SaveAsync(fietstocht);
    }
}
