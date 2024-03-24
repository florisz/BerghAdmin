namespace BerghAdmin.Services.Import;

public interface IImporterService
{
    Task ImportDataAsync(Stream csvData);
}
