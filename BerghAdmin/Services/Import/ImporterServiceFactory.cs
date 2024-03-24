namespace BerghAdmin.Services.Import;

public class ImporterServiceFactory : IImporterServiceFactory
{
    private readonly IEnumerable<IImporterService> importerServices;

    public ImporterServiceFactory(IEnumerable<IImporterService> importerServices)
    {
        this.importerServices = importerServices;
    }

    public IImporterService GetInstance(string identifier)
    {
        return identifier switch
        {
            "Persoon" => this.GetService(typeof(PersoonImporterService)),
            "Ambassadeur" => this.GetService(typeof(AmbassadeurImporterService)),
            _ => throw new InvalidOperationException()
        };
    }

    public IImporterService GetService(Type type)
    {
        return this.importerServices.FirstOrDefault(x => x.GetType() == type)!;
    }
}
