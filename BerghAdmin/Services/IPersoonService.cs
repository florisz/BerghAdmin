using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services
{
    public interface IPersoonService
    {
        List<Persoon>? GetPersonen();
        List<Persoon>? GetFietsersEnBegeleiders();
        Persoon? GetByActionId(int actionId);
        Persoon? GetById(int id);
        Persoon? GetByEmailAdres(string emailAdres);
        Task SavePersoonAsync(Persoon persoon);
        Task DeletePersoonAsync(int id);
    }
}
