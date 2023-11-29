using BerghAdmin.General;

namespace BerghAdmin.Services.Sponsoren;

public interface ISponsorService
{
    Task<ErrorCodeEnum> SaveAsync<T>(Sponsor sponsor) where T : Sponsor;
    Task<T?> GetById<T>(int id) where T : Sponsor;
    Task<T?> GetByNaam<T>(string naam) where T : Sponsor;
    Task<IEnumerable<T>?> GetAll<T>();

    // convenience functions
    Task<IEnumerable<Ambassadeur>?> GetAllAmbassadeurs();
    Task<IEnumerable<GolfdagSponsor>?> GetAllGolfdagSponsoren();
}
