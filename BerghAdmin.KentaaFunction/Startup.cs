using BerghAdmin.ApplicationServices.KentaaInterface;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(BerghAdmin.KentaaFunction.Startup))]

namespace BerghAdmin.KentaaFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(typeof(Startup).Assembly)
            .Build();
        builder.Services.AddOptions()
            .Configure<KentaaConfiguration>(configuration.GetSection("KentaaConfiguration"))
            .Configure<BerghAdminConfiguration>(configuration.GetSection("BerghAdminConfiguration"))
            .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>()
            .AddScoped<BerghAdminService>().AddHttpClient();
    }
}