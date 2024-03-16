using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Bihz;

public interface IBihzDonatieService
{
    Task AddAsync(BihzDonatie donatie);
    Task AddAsync(IEnumerable<BihzDonatie> donaties);
    bool Exist(BihzDonatie bihzDonatie);
    IEnumerable<BihzDonatie>? GetAll();
    BihzDonatie? GetById(int id);             // internal id
    BihzDonatie? GetByKentaaId(int kentaaId); // id as identified by kentaa 
    Task SaveAsync(BihzDonatie bihzDonatie);
}
