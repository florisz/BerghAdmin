namespace BerghAdmin.Services.Facturen;

public interface IFactuurService
{
    Task<Factuur?> GetNewFactuurAsync();
    Task<Factuur?> GetNewFactuurAsync(DateTime dateTime);
    Task<Factuur?> GetNewFactuurAsync(int nummer, DateTime dateTime);
    Task<Factuur?> GetFactuurAsync(int nummer);
    Task<List<Factuur>> GetFacturenAsync(int jaar);
    Task<bool> SaveFactuurAsync(Factuur factuur);
    Task MaakFactuurVoorAmbassadeur(string templateName, Ambassadeur ambassadeur);
}
