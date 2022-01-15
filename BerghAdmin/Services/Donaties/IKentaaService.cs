using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties
{
    public interface IKentaaService
    {
        IEnumerable<KentaaDonatie> GetDonations();
        IEnumerable<KentaaDonatie> GetDonationsFromDate(DateTime startDate);
    }
}
