using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IFietstochtenService
{
    Task<ErrorCodeEnum> SaveAsync(Fietstocht fietstocht);
    IEnumerable<Fietstocht>? GetAll();
    Fietstocht? GetById(int id);
    Fietstocht? GetByTitel(string titel);
    Fietstocht? GetByProjectId(int projectId);
    Fietstocht? GetByProject(BihzProject project);
    Task<ErrorCodeEnum> AddDeelnemerAsync(Fietstocht fietstocht, Persoon persoon);
    Task<ErrorCodeEnum> DeleteDeelnemerAsync(Fietstocht fietstocht, Persoon persoon);
    FietstochtListItem[]? GetAlleFietstochtListItems();
    void SetFietstochten(Persoon persoon, List<FietstochtListItem> fietstochtListItems);

}
