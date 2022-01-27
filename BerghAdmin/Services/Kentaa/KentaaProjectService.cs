using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
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

    public void AddKentaaProject(Project kentaaProject)
    {
        var project = GetByKentaaId(kentaaProject.Id);

        project = MapChanges(project, kentaaProject);

        Save(project);
    }

    public void AddKentaaProjects(IEnumerable<Project> kentaaProjects)
    {
        foreach (var kentaaProject in kentaaProjects)
        {
            AddKentaaProject(kentaaProject);
        }
    }

    public bool Exist(KentaaProject project)
        => GetByKentaaId(project.ProjectId) != null;

    public IEnumerable<KentaaProject>? GetAll()
        => _dbContext
            .KentaaProjects;

    public KentaaProject? GetById(int id)
       => _dbContext
            .KentaaProjects?
            .SingleOrDefault(kp => kp.Id == id);

    public KentaaProject? GetByKentaaId(int kentaaId)
        => _dbContext
            .KentaaProjects?
            .SingleOrDefault(kp => kp.ProjectId == kentaaId);

    public ErrorCodeEnum Save(KentaaProject project)
    {
        try
        {
            if (project.Id == 0)
            {
                _dbContext
                    .KentaaProjects?
                    .Add(project);
            }
            else
            {
                _dbContext
                    .KentaaProjects?
                    .Update(project);
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

    private static KentaaProject MapChanges(KentaaProject? project, Project kentaaProject)
    {
        if (project != null)
        {
            project.Update(kentaaProject);
        }
        else
        {
            project = new KentaaProject(kentaaProject);
        }

        return project;
    }
}
