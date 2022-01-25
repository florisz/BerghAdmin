using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.General;

namespace BerghAdmin.Services.Kentaa;

public interface IKentaaProjectService
{
    void AddKentaaProject(KM.Project kentaaProject);
    bool Exist(KentaaProject project);
    IEnumerable<KentaaProject>? GetAll();
    KentaaProject? GetById(int id);
    KentaaProject? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(KentaaProject donatie);
}
