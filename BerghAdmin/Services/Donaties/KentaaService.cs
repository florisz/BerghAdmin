namespace BerghAdmin.Services.Donaties;

public class KentaaService : IKentaaService
{
    public void AddDonation(KentaaDonatie donation)
    {
    }

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
