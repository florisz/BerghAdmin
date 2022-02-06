namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

public interface IResource
{
}

public interface IResources<IResource>
{
    public IEnumerable<IResource> GetResources();
}
