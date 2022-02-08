namespace BerghAdmin.Services.Betalingen;

public interface IBetalingenImporterService
{
    List<Betaling> ImportBetalingen(Stream csvData);
}
