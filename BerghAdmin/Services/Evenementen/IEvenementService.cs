using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IEvenementService
{
    ErrorCodeEnum SaveEvenement(Evenement evenement);
    Evenement GetById(int id);
    Evenement GetByName(string name);
    IEnumerable<T>? GetAllEvenementen<T>();
}
