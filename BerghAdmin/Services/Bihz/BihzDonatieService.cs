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


    public void AddBihzDonatie(Donation donation)
    {
        _logger.LogDebug("entering AddBihzDonatie");

        var bihzDonatie = GetByKentaaId(donation.Id);

        bihzDonatie = MapChanges(bihzDonatie, donation);

        if (bihzDonatie.PersoonId == null)
        {
            LinkDonatieToPersoon(bihzDonatie);
        }

        Save(bihzDonatie);
    }

    public void AddBihzDonaties(IEnumerable<Donation> donations)
    {
        foreach (var donation in donations)
        {
            AddBihzDonatie(donation);
        }
    }

    public bool Exist(BihzDonatie bihzDonatie)
        => GetByKentaaId(bihzDonatie.DonationId) != null;

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

    public ErrorCodeEnum Save(BihzDonatie bihzDonatie)
    {
        try
        {
            if (bihzDonatie.Id == 0)
            {
                _dbContext
                    .BihzDonaties?
                    .Add(bihzDonatie);
            }
            else
            {
                _dbContext
                    .BihzDonaties?
                    .Update(bihzDonatie);
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

    private BihzDonatie MapChanges(BihzDonatie? bihzDonatie, Donation donation)
    {
        if (bihzDonatie != null)
        {
            bihzDonatie.Map(donation);
        }
        else
        {
            bihzDonatie = new BihzDonatie(donation);
        }

        return bihzDonatie;
    }

    private void LinkDonatieToPersoon(BihzDonatie bihzDonatie)
    {
        _logger.LogDebug("entering LinkDonatieToPersoon");

        // link via action
        var bihzAction = _bihzActieService.GetById(bihzDonatie.ActionId);
        if (bihzAction == null || bihzAction.PersoonId == null)
        {
            _logger.LogError("Donatie can not be linked to persoon");
            // TO BE DONE
            // report admin that donatie can not be linked
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

        if (persoon != null)
        {
            bihzDonatie.PersoonId = persoon.Id;
            _donatieService.ProcessBihzDonatie(bihzDonatie, persoon);
        }
    }

}
