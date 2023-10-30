namespace BerghAdmin.Services.Betalingen;

public interface IBetalingenService
{
    IEnumerable<Betaling>? GetAll();
    Betaling? GetByVolgnummer(string volgNummer);
    Task SaveAsync(Betaling betaling);
}
