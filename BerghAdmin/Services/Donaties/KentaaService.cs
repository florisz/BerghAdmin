using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.DbContexts;

namespace BerghAdmin.Services.Donaties;

public class KentaaService : IKentaaService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDonatieService _donatieService;
    public KentaaService(ApplicationDbContext context, IDonatieService donatieService)
    {
        _dbContext = context;
        _donatieService = donatieService;
    }

    public void ProcessKentaaAction(ApplicationServices.KentaaInterface.KentaaModel.Action kentaaAction)
    {
        throw new NotImplementedException();
    }

    public void ProcessKentaaProject(Project kentaaProject)
    {
        throw new NotImplementedException();
    }

    public void ProcessKentaaUser(ApplicationServices.KentaaInterface.KentaaModel.User kentaaUser)
    {
        throw new NotImplementedException();
    }

    public void ProcessKentaaDonation(Donation kentaaDonation)
    {
        var donatie = _donatieService.GetByKentaaId(kentaaDonation.Id);

        donatie = ProcessChanges(donatie, kentaaDonation);

        _donatieService.Save(donatie);
    }

    private KentaaDonatie ProcessChanges(KentaaDonatie? donatie, Donation kentaaDonation)
    {
        if (donatie != null)
        {
            donatie.Update(kentaaDonation);
        }
        else
        {
            donatie = new KentaaDonatie(kentaaDonation);
        }

        return donatie;
    }
}
