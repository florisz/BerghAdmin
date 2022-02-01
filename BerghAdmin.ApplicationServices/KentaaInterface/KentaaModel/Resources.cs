namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

public interface Resource
{
}

public abstract class Resources<T> where T : Resource
{
    public abstract IEnumerable<T> GetResources();
    public abstract string Endpoint { get; }
}
