namespace BerghAdmin.Services.Betalingen;

public interface IBetalingenImporterService
{
    List<Betaling> ImportData(Stream csvData);
}
