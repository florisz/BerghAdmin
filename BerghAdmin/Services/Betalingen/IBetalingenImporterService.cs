namespace BerghAdmin.Services.Betalingen;

public interface IBetalingenImporterService
{
    void ImportData(Stream csvData);
}
