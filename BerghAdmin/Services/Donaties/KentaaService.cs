using BerghAdmin.Services.Context;

namespace BerghAdmin.Services.Donaties;

public class KentaaService : IKentaaService
{
    IContextService _contextService;

    public KentaaService(IContextService contextService)
    {
        _contextService = contextService;
        var apiKey = _contextService.Context.MailJetConfiguration.ApiKey;
    }

    public IEnumerable<KentaaDonatie> GetDonations()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<KentaaDonatie> GetDonationsFromDate(DateTime startDate)
    {
        throw new NotImplementedException();
    }
}
