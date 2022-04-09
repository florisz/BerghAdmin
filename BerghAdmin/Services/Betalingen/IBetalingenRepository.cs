namespace BerghAdmin.Services.Betalingen
{
    public interface IBetalingenRepository
    {
        void Add(Betaling betaling);
        void Update(Betaling betaling);
        Betaling? GetByVolgnummer(string volgNummer);
        IEnumerable<Betaling>? GetAll();
    }
}