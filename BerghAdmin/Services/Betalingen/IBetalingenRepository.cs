namespace BerghAdmin.Services.Betalingen
{
    public interface IBetalingenRepository
    {
        Task AddAsync(Betaling betaling);
        Task UpdateAsync(Betaling betaling);
        Betaling? GetByVolgnummer(string volgNummer);
        IEnumerable<Betaling>? GetAll();
    }
}