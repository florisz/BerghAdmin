using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

public interface IResource
{
    IBihzResource Map();
}

public interface IResources<IResource>
{
    public IEnumerable<IResource> GetResources();
}
