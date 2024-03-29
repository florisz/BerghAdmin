﻿using BerghAdmin.ApplicationServices.KentaaInterface;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System.Text;
using System.Text.Json;

using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.KentaaTest;

public class KentaaTest
{
    readonly ServiceProvider _serviceProvider;

    public KentaaTest(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ReadKentaaAndSendToBerghdmin()
    {
        var optionValue = new KentaaConfiguration()
        {
            //test kentaa
            //ApiKey = "9793e5718dafb2f50aec5483e8b88dea97d43a237a3cb7dbfa50d2d747285ebb",
            //production kentaa
            ApiKey = "94f9fdcca1c0e574160f2a76675bee77971d5894c91d83d6bc221dbab5055555",
            KentaaBasePath = "v1",
            KentaaHost = "api.kentaa.nl"
        };
        var options = Options.Create<KentaaConfiguration>(optionValue);

        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var kentaaInterfaceService = new KentaaInterfaceService(options, httpClientFactory);
        using (var httpClient = httpClientFactory.CreateClient())
        {
            httpClient.DefaultRequestHeaders.Add("api-key", "abcdefghijklm");
            await ReadAndPostAllResources(kentaaInterfaceService, httpClient);
        }
    }

    public static async Task ReadAndPostAllResources(IKentaaInterfaceService service, HttpClient httpClient)
    {
        const string berghAdminUrl = "https://bergh-test-bergh-admin-webapp.azurewebsites.net";
        //const string berghAdminUrl = "https://localhost:44344";

        var users = service.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter());
        await foreach (var user in users)
        {
            Console.WriteLine($"Post user {user.first_name} {user.last_name} ({user.email})");
            var content = GetContent(user.Map());
            await httpClient.PostAsync($"{berghAdminUrl}/users", content);
        }
        var projects = service.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
        await foreach (var project in projects)
        {
            Console.WriteLine($"Post Project {project.title}");
            var content = GetContent(project.Map());
            await httpClient.PostAsync($"{berghAdminUrl}/projects", content);
        }
        var actions = service.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());
        await foreach (var action in actions)
        {
            Console.WriteLine($"Post Action {action.title}; User: {action.first_name} {action.last_name}; ProjectId={action.project_id}");
            var content = GetContent(action.Map());
            await httpClient.PostAsync($"{berghAdminUrl}/actions", content);
        }
        var donations = service.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
        await foreach (var donation in donations)
        {
            Console.WriteLine($"Post Donation; amount={donation.amount}; receivable={donation.receivable_amount}");
            var content = GetContent(donation.Map());
            await httpClient.PostAsync($"{berghAdminUrl}/donations", content);
        }
    }

    private static StringContent GetContent(object payload) 
        => new(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
}
