using Azure.Identity;

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
        var env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
        var kentaaConfiguration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        
        if (env != "Development")
        {
            kentaaConfiguration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddAzureKeyVault(
                  new Uri($"https://bergh-{env}-keyvault.vault.azure.net"),
                  new DefaultAzureCredential()
            ).Build();
        }

        builder.Services.AddOptions()
            .Configure<KentaaConfiguration>(kentaaConfiguration.GetSection("KentaaConfiguration"))
            .Configure<ApiConfiguration>(kentaaConfiguration.GetSection("ApiConfiguration"))
            .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>()
            .AddScoped<BerghAdminService>().AddHttpClient();

    }
}