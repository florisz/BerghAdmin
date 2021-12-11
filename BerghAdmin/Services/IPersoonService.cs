namespace BerghAdmin.Services
{
    public interface IPersoonService
    {
        List<Persoon> GetPersonen();
        Persoon? GetPersoonById(int id);
        void SavePersoon(Persoon persoon);
        void DeletePersoon(int id);
    }
}
