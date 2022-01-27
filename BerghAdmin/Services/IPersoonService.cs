namespace BerghAdmin.Services
{
    public interface IPersoonService
    {
        List<Persoon>? GetPersonen();
        Persoon? GetById(int id);
        Persoon? GetByEmailAdres(string emailAdres);
        void SavePersoon(Persoon persoon);
        void DeletePersoon(int id);
    }
}
