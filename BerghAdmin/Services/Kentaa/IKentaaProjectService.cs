using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.General;
using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.Services.Kentaa;

public interface IKentaaProjectService
{
    void AddKentaaProject(Project project);
    void AddKentaaProjects(IEnumerable<Project> projects);
    bool Exist(BihzProject bihzProject);
    IEnumerable<BihzProject>? GetAll();
    BihzProject? GetById(int id);
    BihzProject? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(BihzProject bihzProject);
}
