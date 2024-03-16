using BerghAdmin.General;

namespace BerghAdmin.Services.Sponsoren;

public interface IAmbassadeurService
{
    Task<ErrorCodeEnum> SaveAsync(Ambassadeur ambassadeur);
    Task<Ambassadeur?> GetById(int id);
    Task<Ambassadeur?> GetByNaam(string naam);
    Task<IEnumerable<Ambassadeur>?> GetAll();
}
