using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Donaties;
using System.Linq;

namespace BerghAdmin.Services.Bihz;

public class BihzDonatieService : IBihzDonatieService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPersoonService _persoonService;
    private readonly IBihzActieService _bihzActieService;
    private readonly IDonatieService _donatieService;
    private readonly ILogger<BihzDonatieService> _logger;

    public BihzDonatieService(ApplicationDbContext context, 
                                IPersoonService persoonService, 
                                IDonatieService donatieService,
                                IBihzActieService bihzActieService,
                                ILogger<BihzDonatieService> logger)
    {
        _dbContext = context;
        _persoonService = persoonService;
        _donatieService = donatieService;
        _bihzActieService = bihzActieService;
        _logger = logger;
    }


    public void Add(BihzDonatie donatie)
    {
        _logger.LogDebug("entering AddBihzDonatie");

        var currentDonatie = GetByKentaaId(donatie.DonationId);

        currentDonatie = MapChanges(currentDonatie, donatie);

        if (currentDonatie.PersoonId == null)
        {
            LinkDonatieToPersoon(currentDonatie);
        }

        Save(currentDonatie);
    }

    private static BihzDonatie MapChanges(BihzDonatie? currentDonatie, BihzDonatie donatie)
    {
        if (currentDonatie == null)
            return new BihzDonatie(donatie);

        return currentDonatie.UpdateFrom(donatie);
    }

    public void Add(IEnumerable<BihzDonatie> donaties)
    {
        foreach (var donatie in donaties)
        {
            Add(donatie);
        }
    }

    public bool Exist(BihzDonatie donatie)
        => GetByKentaaId(donatie.DonationId) != null;

    public IEnumerable<BihzDonatie>? GetAll() 
        => _dbContext
            .BihzDonaties;

    public BihzDonatie? GetById(int id)
       => _dbContext
            .BihzDonaties?
            .SingleOrDefault(kd => kd.Id == id);

    public BihzDonatie? GetByKentaaId(int kentaaId)
        => _dbContext
            .BihzDonaties?
            .SingleOrDefault(kd => kd.DonationId == kentaaId);

    public ErrorCodeEnum Save(BihzDonatie donatie)
    {
        try
        {
            if (donatie.Id == 0)
            {
                _dbContext
                    .BihzDonaties?
                    .Add(donatie);
            }
            else
            {
                _dbContext
                    .BihzDonaties?
                    .Update(donatie);
            }

            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            // log exception
            return ErrorCodeEnum.Conflict;
        }

        return ErrorCodeEnum.Ok;
    }

    private void LinkDonatieToPersoon(BihzDonatie bihzDonatie)
    {
        _logger.LogDebug("entering LinkDonatieToPersoon");

        // link via action
        var bihzAction = _bihzActieService.GetByKentaaId(bihzDonatie.ActionId);
        if (bihzAction == null)
        {
            _logger.LogError("Donatie not linked to an action; can not link it to a person");
            // TO BE DONE
            // report admin that donatie can not be linked
            return;
        }
        if (bihzAction!.PersoonId == null)
        {
            _logger.LogError("Action not linked to persoon, can not link this incoming donatie");
            return;
        }

        var persoon = _persoonService.GetById((int)bihzAction.PersoonId);

        if (persoon == null)
        {
            _logger.LogError($"Donatie can not be linked, persoon with id {bihzAction.PersoonId} not found");
            // TO BE DONE
            // report to admin "kentaa donation can not be mapped"
            return;
        }

        bihzDonatie.PersoonId = persoon.Id;
        _donatieService.ProcessBihzDonatie(bihzDonatie, persoon);
    }

}
