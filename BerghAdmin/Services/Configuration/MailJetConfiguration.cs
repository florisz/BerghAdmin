#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace BerghAdmin.Services.Configuration;

public class MailJetConfiguration
{
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}
