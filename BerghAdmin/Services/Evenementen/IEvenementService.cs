using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IEvenementService
{
    Task<ErrorCodeEnum> Save(Evenement evenement);
    Evenement? GetById(int id);
    Evenement? GetByName(string name);
    Evenement? GetByProjectId(int projectId);
    IEnumerable<T>? GetAll<T>();
    Task<ErrorCodeEnum> AddDeelnemer(Evenement evenement, Persoon persoon);
    Task<ErrorCodeEnum> AddDeelnemer(Evenement evenement, int persoonId);
    Task<ErrorCodeEnum> DeleteDeelnemer(Evenement evenement, Persoon persoon);
    Task<ErrorCodeEnum> DeleteDeelnemer(Evenement evenement, int persoonId);
}
