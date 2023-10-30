using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IEvenementService
{
    Task<ErrorCodeEnum> SaveAsync(Evenement evenement);
    Evenement? GetById(int id);
    Evenement? GetByTitel(string titel);
    Evenement? GetByProjectId(int projectId);
    Evenement? GetByProject(BihzProject project);
    IEnumerable<T>? GetAll<T>();
    IEnumerable<FietsTocht>? GetAllFietsTochten();
    Task<ErrorCodeEnum> AddDeelnemerAsync(Evenement evenement, Persoon persoon);
    Task<ErrorCodeEnum> AddDeelnemerAsync(Evenement evenement, int persoonId);
    Task<ErrorCodeEnum> DeleteDeelnemerAsync(Evenement evenement, Persoon persoon);
    Task<ErrorCodeEnum> DeleteDeelnemerAsync(Evenement evenement, int persoonId);
}
