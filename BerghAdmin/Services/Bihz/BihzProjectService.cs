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

    public async Task AddAsync(BihzProject project)
    {
        _logger.LogDebug("Add BihzProject with KentaaId {ProjectId}", project.ProjectId);

        var bihzProject = MapChanges(GetByKentaaId(project.ProjectId), project);

        if (bihzProject.EvenementId == null)
        {
            // Evenement (fietstocht) has not been linked to a registered Kentaa project yet,
            // Link thru the title of the Kentaa project
            var fietsTocht = _evenementService.GetByProject(project) as FietsTocht;

            if (fietsTocht == null)
            {
                _logger.LogError("Kentaa project with id {ProjectId} can not be processed; reason: the corresponding evenement with title {Titel} is unknown.",
                        project.ProjectId, project.Titel);
                return;
            }
            bihzProject.EvenementId = fietsTocht.Id;

            fietsTocht.KentaaProjectId = bihzProject.ProjectId;
            await _evenementService.SaveAsync(fietsTocht);
            
            _logger.LogInformation("Kentaa project with id {ProjectId} successfully saved and linked to evenement with id {EvenementId}", bihzProject.ProjectId, bihzProject.EvenementId);
        }

        Save(bihzProject);
        _logger.LogInformation("Values from Kentaa project with id {ProjectId} successfully saved to evenement with id {EvenementId}", bihzProject.ProjectId, bihzProject.EvenementId);
    }

    public async Task AddAsync(IEnumerable<BihzProject> projects)
    {
        foreach (var project in projects)
        {
            await AddAsync(project);
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
