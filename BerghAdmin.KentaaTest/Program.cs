using BerghAdmin.ApplicationServices.KentaaInterface;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BerghAdmin.KentaaTest;

public class Program
{

    public static async Task Main(string[] args)
    {


        var services = new ServiceCollection();

        RegisterServices(services);

        var serviceProvider = services.BuildServiceProvider();
        var configuration = GetConfiguration();

        var test = new KentaaTest(serviceProvider);

        await test.ReadKentaaAndSendToBerghdmin();
    }


    private static IConfigurationRoot GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json", optional: false)
            .Build();
    }

    protected static void RegisterServices(ServiceCollection services)
    {
        services.AddHttpClient()
            .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>();

    }

}