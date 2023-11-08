using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IGolfdagenService
{
    Task<ErrorCodeEnum> AddDeelnemerAsync(Golfdag golfdag, Persoon persoon);
    Task<ErrorCodeEnum> AddDeelnemerAsync(Golfdag golfdag, int persoonId);
    Task<ErrorCodeEnum> AddSponsorAsync(Golfdag golfdag, GolfdagSponsor sponsor);
    Task<ErrorCodeEnum> AddSponsorAsync(Golfdag golfdag, int sponsorId);
    IEnumerable<Golfdag>? GetAll();
    Golfdag? GetById(int id);
    Golfdag? GetByTitel(string titel);
    Task<ErrorCodeEnum> DeleteDeelnemerAsync(Golfdag golfdag, Persoon persoon);
    Task<ErrorCodeEnum> DeleteDeelnemerAsync(Golfdag golfdag, int persoonId);
    Task<ErrorCodeEnum> DeleteSponsorAsync(Golfdag golfdag, GolfdagSponsor sponsor);
    Task<ErrorCodeEnum> DeleteSponsorAsync(Golfdag golfdag, int sponsorId);
    Task<ErrorCodeEnum> SaveAsync(Golfdag golfdag);
}
