using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Evenementen;

namespace BerghAdmin.Services.Bihz;

public class BihzProjectService : IBihzProjectService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEvenementService _evenementService;
    private readonly ILogger<BihzProjectService> _logger;

    public BihzProjectService(ApplicationDbContext context, IEvenementService evenementService, ILogger<BihzProjectService> logger)
    {
        _dbContext = context;
        _evenementService = evenementService;
        _logger = logger;
    }

    public void Add(BihzProject project)
    {
        _logger.LogDebug($"Entering Add BihzProject with KentaaId {project.ProjectId}");

        var bihzProject = MapChanges(GetByKentaaId(project.ProjectId), project);

        if (bihzProject.EvenementId == null)
        {
            // Evenement (fietstocht) has not been linked to a registered Kentaa project yet,
            // Link thru the title of the Kentaa project
            var evenement = _evenementService.GetByTitel(project.Titel ?? "no-title");

            if (evenement == null)
            {
                _logger.LogError($"Kentaa project with id {project.ProjectId} can not be processed; reason: the corresponding evenement with title {project.Titel} is unknown.");
                return;
            }
            evenement.BihzProject = bihzProject;
            bihzProject.EvenementId = evenement.Id;

            _evenementService.Save(evenement);
        }

        Save(bihzProject);
    }

    public void Add(IEnumerable<BihzProject> projects)
    {
        foreach (var project in projects)
        {
            Add(project);
        }
    }

    public bool Exist(BihzProject bihzProject)
        => GetByKentaaId(bihzProject.ProjectId) != null;

    public IEnumerable<BihzProject> GetAll()
        => _dbContext
            .BihzProjects ?? Enumerable.Empty<BihzProject>();

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

    private static BihzProject MapChanges(BihzProject? currentProject, BihzProject newProject)
    {
        if (currentProject == null)
            return new BihzProject(newProject);

        return currentProject.UpdateFrom(newProject);
    }

}
