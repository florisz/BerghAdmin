namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

public abstract class Resource
{
}

public abstract class Resources<T> where T : Resource
{
    public abstract IEnumerable<T> GetIssues();
    public abstract string Endpoint { get; }
}
