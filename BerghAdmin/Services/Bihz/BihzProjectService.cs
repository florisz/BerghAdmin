using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Evenementen;

namespace BerghAdmin.Services.Bihz;

public class BihzProjectService : IBihzProjectService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEvenementService _evenementService;

    public BihzProjectService(ApplicationDbContext context, IEvenementService evenementService)
    {
        _dbContext = context;
        _evenementService = evenementService;
    }

    public void AddBihzProject(KM.Project project)
    {
        var bihzProject = GetByKentaaId(project.Id);

        bihzProject = MapChanges(bihzProject, project);

        if (bihzProject.EvenementId == null)
        {
            LinkProjectToEvenement(bihzProject);
        }

        Save(bihzProject);
    }

    public void AddBihzProjects(IEnumerable<KM.Project> projects)
    {
        foreach (var project in projects)
        {
            AddBihzProject(project);
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

    private void LinkProjectToEvenement(BihzProject bihzProject)
    {
        // link with kentaa user id does not exist yet; try email
        var evenement = _evenementService.GetByTitel(bihzProject.Titel ?? "no-title");

        if (evenement == null)
        {
            // TO BE DONE
            // report to admin "kentaa project can not be mapped"
        }
        if (evenement != null)
        {
            evenement.BihzProject = bihzProject;
            bihzProject.EvenementId = evenement.Id;
            _evenementService.Save(evenement);
        }
    }

}
