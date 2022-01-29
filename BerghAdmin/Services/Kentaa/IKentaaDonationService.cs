using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Kentaa;

public interface IKentaaDonationService
{
    void AddKentaaDonation(Donation donation);
    void AddKentaaDonations(IEnumerable<Donation> donations);
    bool Exist(BihzDonatie bihzDonatie);
    IEnumerable<BihzDonatie>? GetAll();
    BihzDonatie? GetById(int id);             // internal id
    BihzDonatie? GetByKentaaId(int kentaaId); // id as identified by kentaa 
    ErrorCodeEnum Save(BihzDonatie bihzDonatie);
}
