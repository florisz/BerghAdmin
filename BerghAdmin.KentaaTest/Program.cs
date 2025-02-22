using BerghAdmin.ApplicationServices.KentaaInterface;

using Microsoft.Extensions.DependencyInjection;

namespace BerghAdmin.KentaaTest;

public class KentaaTestProgram
{

    public static async Task KentaaTestMainMain(string[] args)
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