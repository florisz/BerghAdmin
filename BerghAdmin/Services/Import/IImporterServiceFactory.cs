namespace BerghAdmin.Services.Import;

public interface IImporterServiceFactory
{ 
    IImporterService GetInstance(string identifier);
}
