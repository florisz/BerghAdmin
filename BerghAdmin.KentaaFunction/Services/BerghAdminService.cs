using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using Microsoft.Extensions.Options;

using System.Net.Http.Json;

namespace BerghAdmin.KentaaFunction.Services;

public class BerghAdminService
{
    private readonly HttpClient berghClient;

    public BerghAdminService(HttpClient httpClient)
    {
        this.berghClient = httpClient;
    }

    public async Task Send<T>(IAsyncEnumerable<KM.User> users)
    {
        await foreach (var user in users)
        {
            await berghClient.PostAsJsonAsync("users", user.Map());
        }
    }

    public async Task Send<T>(IAsyncEnumerable<KM.Project> projects)
    {
        await foreach (var project in projects)
        {
            await berghClient.PostAsJsonAsync("projects", project.Map());
        }
    }

    public async Task Send<T>(IAsyncEnumerable<KM.Action> actions)
    {
        await foreach (var action in actions)
        {
            await berghClient.PostAsJsonAsync("actions", action.Map());
        }
    }
    public async Task Send<T>(IAsyncEnumerable<KM.Donation> donations)
    {
        await foreach (var donation in donations)
        {
            await berghClient.PostAsJsonAsync("donations", donation.Map());
        }
    }
}
