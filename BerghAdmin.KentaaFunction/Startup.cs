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
        builder.Services.AddOptions<KentaaConfiguration>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("KentaaConfiguration").Bind(settings);
            });
        builder.Services.AddScoped<IKentaaInterfaceService, KentaaInterfaceService>();
        builder.Services.AddHttpClient<FunctionApp>();
    }
}