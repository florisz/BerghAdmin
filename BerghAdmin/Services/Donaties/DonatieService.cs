using BerghAdmin.Data.Kentaa;
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

    public ErrorCodeEnum AddDonateur(DonatieBase donatie, Donateur persoon)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddFactuur(DonatieBase donatie, Factuur factuur)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddKentaaDonatie(BihzDonatie bihzDonatie, Donateur? persoon)
    {
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


        return ErrorCodeEnum.Ok;
    }

    public ErrorCodeEnum AddKentaaDonatie(BihzDonatie bihzDonatie)
    {
        return AddKentaaDonatie(bihzDonatie, null);
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
