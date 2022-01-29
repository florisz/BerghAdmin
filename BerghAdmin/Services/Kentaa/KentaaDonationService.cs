using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using System.Linq;

namespace BerghAdmin.Services.Kentaa;

public class KentaaDonationService : IKentaaDonationService
{
    private readonly ApplicationDbContext _dbContext;

    public KentaaDonationService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public void AddKentaaDonation(Donation donation)
    {
        var bihzDonatie = GetByKentaaId(donation.Id);

        bihzDonatie = MapChanges(bihzDonatie, donation);

        Save(bihzDonatie);
    }

    public void AddKentaaDonations(IEnumerable<Donation> donations)
    {
        foreach (var donation in donations)
        {
            AddKentaaDonation(donation);
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
}
