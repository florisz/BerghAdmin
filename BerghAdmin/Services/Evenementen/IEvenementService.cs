using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IEvenementService
{
    Task<ErrorCodeEnum> Save(Evenement evenement);
    Evenement? GetById(int id);
    Evenement? GetByTitel(string titel);
    Evenement? GetByProjectId(int projectId);
    Evenement? GetByProject(BihzProject project);
    IEnumerable<T>? GetAll<T>();
    IEnumerable<FietsTocht>? GetAllFietsTochten();
    Task<ErrorCodeEnum> AddDeelnemer(Evenement evenement, Persoon persoon);
    Task<ErrorCodeEnum> AddDeelnemer(Evenement evenement, int persoonId);
    Task<ErrorCodeEnum> DeleteDeelnemer(Evenement evenement, Persoon persoon);
    Task<ErrorCodeEnum> DeleteDeelnemer(Evenement evenement, int persoonId);
}
