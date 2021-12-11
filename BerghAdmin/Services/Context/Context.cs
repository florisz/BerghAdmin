using BerghAdmin.Services.Configuration;

namespace BerghAdmin.Services.Context;

public class Context
{
    public SyncfusionConfiguration SyncfusionConfiguration { get; set; } = new();
    public MailJetConfiguration MailJetConfiguration { get; set; } = new();
    public User CurrentUser { get; set; } = new();
}
