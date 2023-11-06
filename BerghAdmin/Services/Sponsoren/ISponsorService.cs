using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Sponsoren;

public interface ISponsorService
{
    Task<ErrorCodeEnum> SaveAsync(Sponsor sponsor);
    Sponsor? GetById(int id);
    Sponsor? GetByNaam(string naam);
    IEnumerable<T>? GetAll<T>();
    IEnumerable<Ambassadeur>? GetAllAmbassadeurs();
    IEnumerable<GolfdagSponsor>? GetAllGolfdagSponsoren();
}
