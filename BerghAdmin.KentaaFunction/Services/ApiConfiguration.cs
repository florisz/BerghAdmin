namespace BerghAdmin.KentaaFunction.Services;

public class ApiConfiguration
{
    public Uri Host { get; set; } = new Uri("https://localhost:5001");
    public string Key { get; set; } = string.Empty;
}
