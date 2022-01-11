namespace BerghAdmin.Services.Evenementen;

public interface IEvenementService
{
    void SaveEvenement(Evenement evenement);
    Evenement GetById(int id);
    Evenement GetByName(string name);

}
