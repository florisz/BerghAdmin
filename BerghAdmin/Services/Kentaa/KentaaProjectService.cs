using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;

namespace BerghAdmin.Services.Kentaa;

public class KentaaProjectService : IKentaaProjectService
{
    private readonly ApplicationDbContext _dbContext;

    public KentaaProjectService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public void AddKentaaProject(KM.Project project)
    {
        var bihzProject = GetByKentaaId(project.Id);

        bihzProject = MapChanges(bihzProject, project);

        Save(bihzProject);
    }

    public void AddKentaaProjects(IEnumerable<ApplicationServices.KentaaInterface.KentaaModel.Project> projects)
    {
        foreach (var project in projects)
        {
            AddKentaaProject(project);
        }
    }

    public bool Exist(BihzProject bihzProject)
        => GetByKentaaId(bihzProject.ProjectId) != null;

    public IEnumerable<BihzProject>? GetAll()
        => _dbContext
            .BihzProjects;

    public BihzProject? GetById(int id)
       => _dbContext
            .BihzProjects?
            .SingleOrDefault(kp => kp.Id == id);

    public BihzProject? GetByKentaaId(int kentaaId)
        => _dbContext
            .BihzProjects?
            .SingleOrDefault(kp => kp.ProjectId == kentaaId);

    public ErrorCodeEnum Save(BihzProject bihzProject)
    {
        try
        {
            if (bihzProject.Id == 0)
            {
                _dbContext
                    .BihzProjects?
                    .Add(bihzProject);
            }
            else
            {
                _dbContext
                    .BihzProjects?
                    .Update(bihzProject);
            }

            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            // log exception
            return ErrorCodeEnum.Conflict;
        }

        return ErrorCodeEnum.Ok;
    }

    private static BihzProject MapChanges(BihzProject? bihzProject, KM.Project project)
    {
        if (bihzProject != null)
        {
            bihzProject.Map(project);
        }
        else
        {
            bihzProject = new BihzProject(project);
        }

        return bihzProject;
    }
}
