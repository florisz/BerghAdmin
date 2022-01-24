using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
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

    public void AddKentaaDonation(Donation kentaaDonation)
    {
        var donatie = GetByKentaaId(kentaaDonation.Id);

        donatie = MapChanges(donatie, kentaaDonation);

        Save(donatie);
    }

    public bool Exist(KentaaDonation donatie)
        => GetByKentaaId(donatie.DonationId) != null;

    public IEnumerable<KentaaDonation>? GetAll() 
        => _dbContext
            .KentaaDonations;

    public KentaaDonation? GetById(int id)
       => _dbContext
            .KentaaDonations?
            .SingleOrDefault(kd => kd.Id == id);

    public KentaaDonation? GetByKentaaId(int kentaaId)
        => _dbContext
            .KentaaDonations?
            .SingleOrDefault(kd => kd.DonationId == kentaaId);

    public ErrorCodeEnum Save(KentaaDonation donatie)
    {
        try
        {
            if (donatie.Id == 0)
            {
                _dbContext
                    .KentaaDonations?
                    .Add(donatie);
            }
            else
            {
                _dbContext
                    .KentaaDonations?
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

    private KentaaDonation MapChanges(KentaaDonation? donatie, Donation kentaaDonation)
    {
        if (donatie != null)
        {
            donatie.Update(kentaaDonation);
        }
        else
        {
            donatie = new KentaaDonation(kentaaDonation);
        }

        return donatie;
    }
}
