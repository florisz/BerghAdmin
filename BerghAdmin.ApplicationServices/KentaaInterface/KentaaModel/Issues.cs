namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

public abstract class Issue
{
}

public abstract class Issues<T> where T : Issue
{
    public abstract IEnumerable<T> GetIssues();
    public abstract string Endpoint { get; }
}
