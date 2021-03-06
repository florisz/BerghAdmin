using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Evenementen;

namespace BerghAdmin.Services.Bihz;

public class BihzActieService : IBihzActieService
{
    private readonly ApplicationDbContext dbContext;
    private readonly ILogger<BihzActieService> _logger;
    private readonly IPersoonService _persoonService;
    private readonly IEvenementService _evenementService;

    public BihzActieService(ApplicationDbContext context, IPersoonService persoonService, IEvenementService evenementService, ILogger<BihzActieService> logger)
    {
        this.dbContext = context;
        this._persoonService = persoonService;
        this._evenementService = evenementService;
        this._logger = logger;
    }

    public void Add(BihzActie actie)
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

            _persoonService.SavePersoon(persoon);

            // Add this persoon as deelnemer of the fietstocht (= project in Kentaa)
            if (bihzActie.ProjectId != null)
            {
                var evenement = _evenementService.GetByProjectId((int) bihzActie.ProjectId);
                if (evenement != null)
                {
                    _logger.LogDebug("Add deelnemer {PersoonNaam} to evenement {EvenementNaam}", persoon.VolledigeNaam, evenement.Titel);
                    _evenementService.AddDeelnemer(evenement, persoon).Wait();
                }
            }
        }

        Save(bihzActie);
        _logger.LogInformation("Kentaa actie with id {ActionId} successfully linked to persoon with id {PersoonId}", bihzActie.ActionId, bihzActie.PersoonId);
    }

    private static BihzActie MapChanges(BihzActie? currentActie, BihzActie newActie)
    {
        if (currentActie == null)
            return new BihzActie(newActie);

        return currentActie.UpdateFrom(newActie);
    }

    public void Add(IEnumerable<BihzActie> actions)
    {
        foreach (var action in actions)
        {
            Add(action);
        }
    }

    public bool Exist(BihzActie bihzActie)
        => GetByKentaaId(bihzActie.ActionId) != null;

    public IEnumerable<BihzActie>? GetAll()
        => dbContext
            .BihzActies;

    public BihzActie? GetById(int id)
       => dbContext
            .BihzActies?
            .SingleOrDefault(kd => kd.Id == id);

    public BihzActie? GetByKentaaId(int kentaaId)
        => dbContext
            .BihzActies?
            .SingleOrDefault(ka => ka.ActionId == kentaaId);

    public ErrorCodeEnum Save(BihzActie bihzActie)
    {
        try
        {
            if (bihzActie.Id == 0)
            {
                dbContext
                    .BihzActies?
                    .Add(bihzActie);
            }
            else
            {
                dbContext
                    .BihzActies?
                    .Update(bihzActie);
            }

            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            // log exception
            return ErrorCodeEnum.Conflict;
        }

        return ErrorCodeEnum.Ok;
    }

}
