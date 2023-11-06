using BerghAdmin.General;

namespace BerghAdmin.Services.Evenementen;

public interface IGolfdagenService
{
    Task<ErrorCodeEnum> AddDeelnemerAsync(Golfdag golfdag, Persoon persoon);
    Task<ErrorCodeEnum> AddDeelnemerAsync(Golfdag golfdag, int persoonId);
    IEnumerable<Golfdag>? GetAll();
    Golfdag? GetById(int id);
    Golfdag? GetByTitel(string titel);
    Task<ErrorCodeEnum> DeleteDeelnemerAsync(Golfdag golfdag, Persoon persoon);
    Task<ErrorCodeEnum> DeleteDeelnemerAsync(Golfdag golfdag, int persoonId);
    Task<ErrorCodeEnum> SaveAsync(Golfdag golfdag);
}
