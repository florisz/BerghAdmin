using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Bihz;

public interface IBihzDonatieService
{
    void Add(BihzDonatie donation);
    void Add(IEnumerable<BihzDonatie> donations);
    bool Exist(BihzDonatie bihzDonatie);
    IEnumerable<BihzDonatie>? GetAll();
    BihzDonatie? GetById(int id);             // internal id
    BihzDonatie? GetByKentaaId(int kentaaId); // id as identified by kentaa 
    ErrorCodeEnum Save(BihzDonatie bihzDonatie);
}
