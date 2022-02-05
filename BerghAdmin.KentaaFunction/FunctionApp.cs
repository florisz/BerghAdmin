using BerghAdmin.ApplicationServices.KentaaInterface;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.KentaaFunction;
public class FunctionApp
{
    private readonly IKentaaInterfaceService service;
    private readonly HttpClient berghClient;

    public FunctionApp(IHttpClientFactory httpClientFactory, IKentaaInterfaceService service)
    {
        this.service = service;
        this.berghClient = httpClientFactory.CreateClient();
    }

    [FunctionName(nameof(ReadDonations))]
    public async Task ReadDonations([TimerTrigger("* * 2 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        var donations = service.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
        await foreach (var donation in donations)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/donaties", donation.Map());
        }
    }

    [FunctionName(nameof(ReadProjects))]
    public async Task ReadProjects([TimerTrigger("* * 2 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        var projects = service.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
        await foreach (var project in projects)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/projects", project.Map());
        }
    }

    [FunctionName(nameof(ReadActions))]
    public async Task ReadActions([TimerTrigger("* * 2 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        var actions = service.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());
        await foreach (var action in actions)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/actions", action.Map());
        }
    }

    [FunctionName(nameof(ReadUsers))]
    public async Task ReadUsers([TimerTrigger("* * 2 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        var users = service.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter());
        await foreach (var user in users)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/user", user.Map());
        }
    }
}
