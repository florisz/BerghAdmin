using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.General;
using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.Services.Bihz;

public interface IBihzProjectService 
{
    Task AddAsync(BihzProject project);
    Task AddAsync(IEnumerable<BihzProject> projects);
    bool Exist(BihzProject bihzProject);
    IEnumerable<BihzProject> GetAll();
    BihzProject? GetById(int id);
    BihzProject? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(BihzProject bihzProject);
}
