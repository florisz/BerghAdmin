namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

public abstract class Issues
{
    public abstract IEnumerable<Issue> GetIssues<Issue>();
    public abstract string Endpoint { get; }
}
