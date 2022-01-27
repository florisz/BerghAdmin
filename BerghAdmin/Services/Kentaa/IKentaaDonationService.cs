using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.General;

namespace BerghAdmin.Services.Kentaa;

public interface IKentaaDonationService
{
    void AddKentaaDonation(Donation kentaaDonation);
    void AddKentaaDonations(IEnumerable<Donation> kentaaDonations);
    bool Exist(KentaaDonation donatie);
    IEnumerable<KentaaDonation>? GetAll();
    KentaaDonation? GetById(int id);
    KentaaDonation? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(KentaaDonation donatie);
}
