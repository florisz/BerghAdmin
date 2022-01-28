using BerghAdmin.DbContexts;
using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public class DonatieService : IDonatieService
{
    private readonly ApplicationDbContext _dbContext;

    public DonatieService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public ErrorCodeEnum AddDonateur(Donatie donatie, Donateur persoon)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddKentaaDonatie(KentaaDonation kentaaDonatie, Donateur persoon)
    {
        if (kentaaDonatie.BetaalStatus != PaymentStatusEnum.Paid)
        {
            return ErrorCodeEnum.Forbidden;
        }

        var donatie = new Donatie()
        {
            Bedrag = kentaaDonatie.DonatieBedrag,
            Donateur = persoon,
            KentaaDonatie = kentaaDonatie
        };


        return ErrorCodeEnum.Ok;
    }

    public ErrorCodeEnum AddKentaaDonatie(KentaaDonation kentaaDonatie)
    {
        return AddKentaaDonatie(kentaaDonatie, null);
    }

    public IEnumerable<Donatie> GetAll()
    {
        throw new NotImplementedException();
    }

    public Donatie? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Save(Donatie donatie)
    {
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
