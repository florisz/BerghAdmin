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

    public ErrorCodeEnum AddDonateur(Donatie donatie, Donateur donateur)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie, Donateur? donateur)
    {
        if (donateur == null)
        {
            _logger.LogError("Donatie with Kentaa id {DonationId} can not be processed; reason Persoon (Donateur) is unknown (has null value)", bihzDonatie.DonationId);
        }

        _logger.LogDebug("betaalstatus={status}", bihzDonatie.BetaalStatus);
        if (bihzDonatie.BetaalStatus != PaymentStatusEnum.Paid)
        {
            return ErrorCodeEnum.Forbidden;
        }

        // check if donatie already exists
        var donatie = GetByKentaaId(bihzDonatie.Id);
        if (donatie == null)
        {
            donatie = new Donatie();
        }
        donatie.Bedrag = bihzDonatie.DonatieBedrag;
        donatie.DatumTijd = bihzDonatie.CreatieDatum;
        donatie.Donateur = donateur;
        donatie.KentaaDonatie = bihzDonatie;

        Save(donatie);
        _logger.LogInformation("Kentaa donatie with id {DonationId} successfully linked to persoon with id {donateurId}", bihzDonatie.DonationId, donateur?.Id);

        return ErrorCodeEnum.Ok;
    }

    public ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie)
    {
        return ProcessBihzDonatie(bihzDonatie, null);
    }

    public IEnumerable<Donatie>? GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Donatie>? GetAll(Donateur donateur)
        => _dbContext
            .Donaties?
            .Include(d => d.Donateur)
            .Where(d => d.Donateur == donateur);


    public Donatie? GetById(int id)
        => _dbContext
            .Donaties?
            .FirstOrDefault(d => d.Id == id);

    public Donatie? GetByKentaaId(int kentaaDonatieId)
        => _dbContext
            .Donaties?
            .Where(d => d.KentaaDonatie != null)
            .FirstOrDefault(d => d.KentaaDonatie!.Id == kentaaDonatieId);

    
    public void Save(Donatie donatie)
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
