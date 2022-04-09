using BerghAdmin.ApplicationServices.KentaaInterface;
using BerghAdmin.KentaaFunction.Services;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BerghAdmin.KentaaFunction.Startup))]

namespace BerghAdmin.KentaaFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var kentaaConfiguration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var berghConfiguration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        builder.Services.AddOptions()
            .Configure<KentaaConfiguration>(kentaaConfiguration.GetSection("KentaaConfiguration"))
            .Configure<BerghAdminConfiguration>(berghConfiguration.GetSection("BerghAdminConfiguration"))
            .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>()
            .AddScoped<BerghAdminService>().AddHttpClient();

    }
}