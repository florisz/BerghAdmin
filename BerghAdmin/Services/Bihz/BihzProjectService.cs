using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Evenementen;

namespace BerghAdmin.Services.Bihz;

public class BihzProjectService : IBihzProjectService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IFietstochtenService _fietstochtenService;
    private readonly ILogger<BihzProjectService> _logger;

    public BihzProjectService(ApplicationDbContext context, IFietstochtenService fietstochtenService, ILogger<BihzProjectService> logger)
    {
        _dbContext = context;
        _fietstochtenService = fietstochtenService;
        _logger = logger;
    }

    public async Task AddAsync(BihzProject project)
    {
        _logger.LogDebug($"AddAsync BihzProject with KentaaId {project.ProjectId}");

        var bihzProject = MapChanges(GetByKentaaId(project.ProjectId), project);

        if (bihzProject.FietstochtId == null)
        {
            // Fietstocht has not been linked to a registered Kentaa project yet,
            // Link thru the title of the Kentaa project
            var fietsTocht = await _fietstochtenService.GetByProject(project);

            if (fietsTocht == null)
            {
                _logger.LogError($"Kentaa project with id {project.ProjectId} can not be processed; reason: the corresponding fietstocht with title {project.Titel} is unknown.");
                return;
            }
            bihzProject.FietstochtId = fietsTocht.Id;

            fietsTocht.KentaaProjectId = bihzProject.ProjectId;
            await _fietstochtenService.SaveAsync(fietsTocht);
            
            _logger.LogInformation($"Kentaa project with id {bihzProject.ProjectId} successfully saved and linked to fietstocht with id {bihzProject.FietstochtId}");
        }

        Save(bihzProject);
        _logger.LogInformation($"Values from Kentaa project with id {bihzProject.ProjectId} successfully saved to evenement with id {bihzProject.FietstochtId}");
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
