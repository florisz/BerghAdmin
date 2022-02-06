namespace BerghAdmin.Services
{
    public interface IPersoonService
    {
        List<Persoon>? GetPersonen();
        List<Persoon>? GetFietsers();
        Persoon? GetByActionId(int actionId);
        Persoon? GetById(int id);
        Persoon? GetByEmailAdres(string emailAdres);
        void SavePersoon(Persoon persoon);
        void DeletePersoon(int id);
    }
}
