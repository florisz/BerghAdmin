using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Sponsoren;

public interface ISponsorService
{
    Task<ErrorCodeEnum> SaveAsync<T>(Sponsor sponsor) where T : Sponsor;
    T? GetById<T>(int id) where T : Sponsor;
    T? GetByNaam<T>(string naam) where T : Sponsor;
    IEnumerable<T>? GetAll<T>();

    // convenience functions
    IEnumerable<Ambassadeur>? GetAllAmbassadeurs();
    IEnumerable<GolfdagSponsor>? GetAllGolfdagSponsoren();
}
