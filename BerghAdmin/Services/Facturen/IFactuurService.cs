namespace BerghAdmin.Services.Facturen;

public interface IFactuurService
{
    Task<Factuur?> GetNewFactuurAsync(Ambassadeur ambassadeur);
    Task<Factuur?> GetNewFactuurAsync(DateTime dateTime, Ambassadeur ambassadeur);
    Task<Factuur?> GetNewFactuurAsync(int nummer, DateTime dateTime, Ambassadeur ambassadeur);
    Task<Factuur?> GetFactuurAsync(int nummer);
    Task<Factuur?> GetFactuurWithPdfAsync(int nummer);
    Task<List<Factuur>> GetFacturenAsync(int jaar);
    Task<bool> SaveFactuurAsync(Factuur factuur, Ambassadeur ambassadeur);
    Task MaakFactuurVoorAmbassadeur(string templateName, Ambassadeur ambassadeur);
}
