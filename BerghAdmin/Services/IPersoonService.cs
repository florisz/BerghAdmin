namespace BerghAdmin.Services
{
    public interface IPersoonService
    {
        Task<List<Persoon>> GetPersonen();
        Task<PersoonListItem[]> GetFietstochtDeelnemers();
        Task<PersoonListItem[]> GetContactPersonen();
        Task<PersoonListItem[]> GetCompagnons();
        Task<Persoon?> GetByActionId(int actionId);
        Task<Persoon?> GetById(int id);
        Task<Persoon?> GetByEmailAdres(string emailAdres);
        Task SavePersoonAsync(Persoon persoon);
        Task DeletePersoonAsync(int id);
        void SetRollen(Persoon persoon, List<RolListItem> rolListItems);
    }
}
