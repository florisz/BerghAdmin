using BerghAdmin.Services.Configuration;

using Microsoft.Extensions.Options;

namespace BerghAdmin.Services.Donaties;

public class KentaaService : IKentaaService
{
    public IEnumerable<KentaaDonatie> GetDonaties()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<KentaaDonatie> GetDonaties(Donateur persoon)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<KentaaDonatie> GetDonaties(Evenement evenement)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<KentaaDonatie> GetDonationsFromDate(DateTime startDate)
    {
        throw new NotImplementedException();
    }
}
