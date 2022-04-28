using Azure.Identity;

using BerghAdmin.ApplicationServices.KentaaInterface;
using BerghAdmin.KentaaFunction.Services;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Net.Http.Headers;

[assembly: FunctionsStartup(typeof(BerghAdmin.KentaaFunction.Startup))]

namespace BerghAdmin.KentaaFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
        if (string.IsNullOrEmpty(env))
            throw new ArgumentNullException("AZURE_FUNCTIONS_ENVIRONMENT not set");

        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        
        if (env != "Development")
        {
            config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddAzureKeyVault(
                  new Uri($"https://bergh-{env}-keyvault.vault.azure.net"),
                  new DefaultAzureCredential()
            ).Build();
        }

        builder.Services.AddOptions()
            .Configure<KentaaConfiguration>(config.GetSection("KentaaConfiguration"))
            .Configure<ApiConfiguration>(config.GetSection("ApiConfiguration"))
            .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>()
            .AddHttpClient<BerghAdminService>(client =>
            {
                client.DefaultRequestHeaders.Add("api-key", config["ApiConfiguration:Key"]);
                client.BaseAddress = new Uri(config["ApiConfiguration:Host"]);
            });

    }
}