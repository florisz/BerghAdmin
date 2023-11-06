using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Evenementen;

namespace BerghAdmin.Services.Bihz;

public class BihzActieService : IBihzActieService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BihzActieService> _logger;
    private readonly IPersoonService _persoonService;
    private readonly IFietstochtenService _fietstochtenService;

    public BihzActieService(ApplicationDbContext dbContext, IPersoonService persoonService, IFietstochtenService fietstochtenService, ILogger<BihzActieService> logger)
    {
        _dbContext = dbContext;
        _persoonService = persoonService;
        _fietstochtenService = fietstochtenService;
        _logger = logger;
    }

    public async Task AddAsync(BihzActie actie)
    {
        _logger.LogDebug("Entering Add BihzActie with KentaaId {KentaaActionId}", actie.ActionId);

        var bihzActie = MapChanges(GetByKentaaId(actie.ActionId), actie);

        if (bihzActie.PersoonId == null)
        {
            // Persoon has not been linked to a registered Kentaa action yet,
            // Link thru the email address of the Kentaa action
            var persoon = _persoonService.GetByEmailAdres(bihzActie.Email ?? "no-email");

            if (persoon == null)
            {
                _logger.LogError("Kentaa actie with id {ActionId} can not be processed; reason: the corresponding persoon with email address {Email} is unknown.",
                        bihzActie.ActionId, bihzActie.Email);
                return;
            }

            persoon.BihzActie = bihzActie;
            bihzActie.PersoonId = persoon.Id;

            await _persoonService.SavePersoonAsync(persoon);

            // Add this persoon as deelnemer of the fietstocht (= project in Kentaa)
            if (bihzActie.ProjectId != null)
            {
                var fietstocht = _fietstochtenService.GetByProjectId((int) bihzActie.ProjectId);
                if (fietstocht != null)
                {
                    _logger.LogDebug("Add deelnemer {PersoonNaam} to fietstocht {EvenementNaam}", persoon.VolledigeNaam, fietstocht.Titel);
                    await _fietstochtenService.AddDeelnemerAsync(fietstocht, persoon);
                }
            }
        }

        await SaveAsync(bihzActie);
        _logger.LogInformation("Kentaa actie with id {ActionId} successfully linked to persoon with id {PersoonId}", bihzActie.ActionId, bihzActie.PersoonId);
    }

    private static BihzActie MapChanges(BihzActie? currentActie, BihzActie newActie)
    {
        if (currentActie == null)
            return new BihzActie(newActie);

        return currentActie.UpdateFrom(newActie);
    }

    public async Task AddAsync(IEnumerable<BihzActie> actions)
    {
        foreach (var action in actions)
        {
            await AddAsync(action);
        }
    }

    public bool Exist(BihzActie bihzActie)
        => GetByKentaaId(bihzActie.ActionId) != null;

    public IEnumerable<BihzActie>? GetAll()
    {
        return _dbContext
            .BihzActies;
    }

    public BihzActie? GetById(int id)
    {
        return _dbContext
            .BihzActies?
            .SingleOrDefault(kd => kd.Id == id);
    }

    public BihzActie? GetByKentaaId(int kentaaId)
    {
        return _dbContext
            .BihzActies?
            .SingleOrDefault(ka => ka.ActionId == kentaaId);
    }

    public async Task<ErrorCodeEnum> SaveAsync(BihzActie bihzActie)
    {
        try
        {
            if (bihzActie.Id == 0)
            {
                _dbContext
                    .BihzActies?
                    .Add(bihzActie);
            }
            else
            {
                _dbContext
                    .BihzActies?
                    .Update(bihzActie);
            }

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            // TOBEDONE log exception
            return ErrorCodeEnum.Conflict;
        }

        return ErrorCodeEnum.Ok;
    }

}
