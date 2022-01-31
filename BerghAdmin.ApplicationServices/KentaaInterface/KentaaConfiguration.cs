namespace BerghAdmin.ApplicationServices.KentaaInterface;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class KentaaConfiguration
{
    public const string Kentaa = "KentaaConfiguration";
    public string ApiKey { get; set; }
    public string KentaaHost { get; set; }
    public string KentaaBasePath { get; set; }
}
