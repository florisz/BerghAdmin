using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using Microsoft.EntityFrameworkCore;

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

    public ErrorCodeEnum AddDonateur(DonatieBase donatie, Donateur donateur)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddFactuur(DonatieBase donatie, Factuur factuur)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie, Donateur? donateur)
    {
        _logger.LogDebug("entering ProcessBihzDonatie");

        _logger.LogDebug($"betaalstatus={bihzDonatie.BetaalStatus}");
        if (bihzDonatie.BetaalStatus != PaymentStatusEnum.Paid)
        {
            return ErrorCodeEnum.Forbidden;
        }

        // check if donatie already exists
        if (GetByKentaaId(bihzDonatie.Id) == null)
        {
            var donatie = new DonatieBase()
            {
                Bedrag = bihzDonatie.DonatieBedrag,
                Donateur = donateur,
                KentaaDonatie = bihzDonatie
            };
            Save(donatie);
        }


        return ErrorCodeEnum.Ok;
    }

    public ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie)
    {
        return ProcessBihzDonatie(bihzDonatie, null);
    }

    public IEnumerable<DonatieBase>? GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DonatieBase>? GetAll(Donateur donateur)
        => _dbContext
            .Donaties?
            .Include(d => d.Donateur)
            .Where(d => d.Donateur == donateur);


    public DonatieBase? GetById(int id)
        => _dbContext
            .Donaties?
            .FirstOrDefault(d => d.Id == id);

    public DonatieBase? GetByKentaaId(int kentaaDonatieId)
        => _dbContext
            .Donaties?
            .Where(d => d.KentaaDonatie != null)
            .FirstOrDefault(d => d.KentaaDonatie.Id == kentaaDonatieId);

    
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
