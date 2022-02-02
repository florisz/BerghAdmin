using BerghAdmin.ApplicationServices.KentaaInterface;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using System;

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
        var donaties = service.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
        await foreach (var donatie in donaties)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/donaties", donatie);
        }
    }

    [FunctionName(nameof(ReadProjects))]
    public async Task ReadProjects([TimerTrigger("* * 2 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        var resources = service.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
        await foreach (var resource in resources)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/projects", resource);
        }
    }

    [FunctionName(nameof(ReadActions))]
    public async Task ReadActions([TimerTrigger("* * 2 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        var resources = service.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());
        await foreach (var resource in resources)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/actions", resource);
        }
    }

    [FunctionName(nameof(ReadUsers))]
    public async Task ReadUsers([TimerTrigger("* * 2 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
        var resources = service.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter());
        await foreach (var resource in resources)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/user", resource);
        }
    }
}
