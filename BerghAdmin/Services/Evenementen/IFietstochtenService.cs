using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IFietstochtenService
{
    Task<ErrorCodeEnum> SaveAsync(Fietstocht fietstocht);
    Task<List<Fietstocht>> GetAll();
    Task<Fietstocht?> GetById(int id);
    Task<Fietstocht?> GetByTitel(string titel);
    Task<Fietstocht?> GetByProjectId(int projectId);
    Task<Fietstocht?> GetByProject(BihzProject project);
    Task<ErrorCodeEnum> AddDeelnemerAsync(Fietstocht fietstocht, Persoon persoon);
    Task<ErrorCodeEnum> DeleteDeelnemerAsync(Fietstocht fietstocht, Persoon persoon);
    Task<FietstochtListItem[]> GetAlleFietstochtListItems();
    Task SetFietstochten(Persoon persoon, List<FietstochtListItem> fietstochtListItems);

}
