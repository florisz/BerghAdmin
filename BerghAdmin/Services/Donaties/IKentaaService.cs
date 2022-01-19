using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties
{
    public interface IKentaaService
    {
        IEnumerable<KentaaDonatie> GetDonaties();
        IEnumerable<KentaaDonatie> GetDonaties(Donateur persoon);
        IEnumerable<KentaaDonatie> GetDonaties(Evenement evenement);
        IEnumerable<KentaaDonatie> GetDonationsFromDate(DateTime startDate);
        void AddDonation(KentaaDonatie donation);
    }
}
