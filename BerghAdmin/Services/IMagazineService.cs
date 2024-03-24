namespace BerghAdmin.Services;

public interface IMagazineService
{
    List<MagazineJaar> GetMagazines();
    MagazineJaar? GetMagazineById(int id);
    MagazineJaar? GetMagazineByJaar(string jaar);
    Task AddMagazine(MagazineJaar magazine);
    Task DeleteAll();
}
