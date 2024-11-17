namespace BerghAdmin.Services.Facturen;

public interface IFactuurService
{
    Task<Factuur?> GetNewFactuurAsync(Ambassadeur ambassadeur);
    Task<Factuur?> GetNewFactuurAsync(DateTime dateTime, Ambassadeur ambassadeur);
    Task<Factuur?> GetNewFactuurAsync(int nummer, DateTime dateTime, Ambassadeur ambassadeur);
    Task<Factuur?> GetFactuurByNummerAsync(int nummer);
    Task<Factuur?> GetFactuurByNummerWithPdfAsync(int nummer);
    Task<Factuur?> GetFactuurByIdAsync(int id);
    Task<Factuur?> GetFactuurByIdWithPdfAsync(int id);
    Task<List<Factuur>> GetFacturenAsync(int jaar);
    Task<bool> SaveFactuurAsync(Factuur factuur, Ambassadeur ambassadeur);
    Task MaakFactuurVoorAmbassadeur(string templateName, Ambassadeur ambassadeur);
}
