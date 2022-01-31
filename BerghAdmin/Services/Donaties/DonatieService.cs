using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public class DonatieService : IDonatieService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DonatieService> _logger;

    public DonatieService(ApplicationDbContext context, ILogger<DonatieService> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public ErrorCodeEnum AddDonateur(DonatieBase donatie, Donateur persoon)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddFactuur(DonatieBase donatie, Factuur factuur)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie, Donateur? persoon)
    {
        _logger.LogDebug("entering ProcessBihzDonatie");

        _logger.LogDebug($"betaalstatus={bihzDonatie.BetaalStatus}");
        if (bihzDonatie.BetaalStatus != PaymentStatusEnum.Paid)
        {
            return ErrorCodeEnum.Forbidden;
        }

        var donatie = new DonatieBase()
        {
            Bedrag = bihzDonatie.DonatieBedrag,
            Donateur = persoon,
            KentaaDonatie = bihzDonatie
        };

        Save(donatie);

        return ErrorCodeEnum.Ok;
    }

    public ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie)
    {
        return ProcessBihzDonatie(bihzDonatie, null);
    }

    public IEnumerable<DonatieBase> GetAll()
    {
        throw new NotImplementedException();
    }

    public DonatieBase? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Save(DonatieBase donatie)
    {
        _logger.LogDebug("entering Save");

        if (donatie.Id == 0)
        {
            _dbContext
                .Donaties?
                .Add(donatie);
        }
        else
        {
            _dbContext
                .Donaties?
                .Update(donatie);
        }
        _dbContext.SaveChanges();
    }
}
