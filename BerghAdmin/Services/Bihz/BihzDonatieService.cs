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
        Persoon? persoon = null;

        _logger.LogDebug("Entering Add BihzDonatie with KentaaId {donatieId}", donatie.DonationId);

        var bihzDonatie = MapChanges(GetByKentaaId(donatie.DonationId), donatie);

        if (bihzDonatie.PersoonId == null)
        {
            // link via action
            var bihzActie = _bihzActieService.GetByKentaaId(bihzDonatie.ActionId);
            if (bihzActie == null)
            {
                _logger.LogError("Kentaa donatie with id {DonationId} can not be processed succesfully; reason: the Kentaa donatie is linked to an unknown Kentaa action with id {ActionId}", 
                        donatie.DonationId, bihzDonatie.ActionId);
            }
            else
            {
                if (bihzActie!.PersoonId == null)
                {
                    _logger.LogError("Kentaa donatie with id {DonationId} can not be processed succesfully; reason: actie with id {bihzActieId} is not linked to a persoon.",
                            donatie.DonationId, bihzActie.Id);
                }

                persoon = _persoonService.GetById((int)bihzActie.PersoonId);
                if (persoon == null)
                {
                    _logger.LogWarning("Kentaa donatie with id {DonationId} can not be processed succesfully; reason: donatie is linked to an unknown persoon with id {PersoonId}.",
                            donatie.DonationId, bihzActie.PersoonId);
                }
                else
                {
                    bihzDonatie.PersoonId = persoon.Id;
                }
            }
        }

        _donatieService.ProcessBihzDonatie(bihzDonatie, persoon);

        Save(bihzDonatie);
        _logger.LogInformation("Kentaa donatie with id {DonationId} successfully linked to persoon with id {PersoonId}", bihzDonatie.DonationId, bihzDonatie.PersoonId);
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

}
