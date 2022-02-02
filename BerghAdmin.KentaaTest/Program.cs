using BerghAdmin.ApplicationServices.KentaaInterface;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BerghAdmin.KentaaTest;

public class Program
{

    public static async Task Main()
    {
        var services = new ServiceCollection();

        RegisterServices(services);

        var serviceProvider = services.BuildServiceProvider();
        var test = new KentaaTest(serviceProvider);

        await test.ReadKentaaAndSendToBerghdmin();
    }

    protected static void RegisterServices(ServiceCollection services)
    {
        services.AddHttpClient()
            .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>();

    }

}