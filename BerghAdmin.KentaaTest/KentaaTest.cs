using BerghAdmin.ApplicationServices.KentaaInterface;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace BerghAdmin.KentaaTest;

public class KentaaTest
{
    ServiceProvider _serviceProvider;

    public KentaaTest(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ReadKentaaAndSendToBerghdmin()
    {
        var optionValue = new KentaaConfiguration()
        {
            ApiKey = "9793e5718dafb2f50aec5483e8b88dea97d43a237a3cb7dbfa50d2d747285ebb",
            KentaaBasePath = "v1",
            KentaaHost = "api.kentaa.nl"
        };
        var options = Options.Create<KentaaConfiguration>(optionValue);

        var httpClientFactory = _serviceProvider.GetService<IHttpClientFactory>();
        var kentaaInterfaceService = new KentaaInterfaceService(options, httpClientFactory);
        using (var httpClient = httpClientFactory.CreateClient())
        {
            await ReadAndPostAllResources(kentaaInterfaceService, httpClient);
        }
    }

    public async Task ReadAndPostAllResources(IKentaaInterfaceService service, HttpClient httpClient)
    {
        var users = service.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter());
        await foreach (var user in users)
        {
            Console.WriteLine($"Post user {user.FirstName} {user.LastName} ({user.Email})");
            var content = GetContent(user);
            await httpClient.PostAsync("https://localhost:44344/users", content);
        }
        var projects = service.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
        await foreach (var project in projects)
        {
            Console.WriteLine($"Post Project {project.Title} {project.Description}");
            var content = GetContent(project);
            await httpClient.PostAsync("https://localhost:44344/projects", content);
        }
        var actions = service.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());
        await foreach (var action in actions)
        {
            Console.WriteLine($"Post Action {action.title} {action.description}");
            var content = GetContent(action);
            await httpClient.PostAsync("https://localhost:44344/actions", content);
        }
        var donations = service.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
        await foreach (var donation in donations)
        {
            Console.WriteLine($"Post Donation; amount={donation.Amount}; receivable={donation.ReceivableAmount}");
            var content = GetContent(donation);
            await httpClient.PostAsync("https://localhost:44344/donations", content);
        }
    }

    private StringContent GetContent(object payload) => new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
}
