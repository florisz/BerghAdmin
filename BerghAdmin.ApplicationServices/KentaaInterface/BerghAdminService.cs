﻿using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using Microsoft.Extensions.Options;

using System.Net.Http.Json;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public class BerghAdminService
{
    private readonly HttpClient berghClient;
    private readonly BerghAdminConfiguration settings;

    public BerghAdminService(HttpClient httpClient, IOptions<BerghAdminConfiguration> settings)
    {
        this.berghClient = httpClient;
        this.settings = settings.Value;
    }

    public async Task Send<T>(IAsyncEnumerable<Donation> donations)
    {
        await foreach (var donation in donations)
        {
            await berghClient.PostAsJsonAsync($"{settings.Host}/donations", donation.Map());
        }
    }

    public async Task Send<T>(IAsyncEnumerable<KentaaModel.Action> actions)
    {
        await foreach (var action in actions)
        {
            await berghClient.PostAsJsonAsync($"{settings.Host}/actions", action.Map());
        }
    }

    public async Task Send<T>(IAsyncEnumerable<Project> projects)
    {
        await foreach (var project in projects)
        {
            await berghClient.PostAsJsonAsync($"{settings.Host}/projects", project.Map());
        }
    }

    public async Task Send<T>(IAsyncEnumerable<User> users)
    {
        await foreach (var user in users)
        {
            await berghClient.PostAsJsonAsync($"{settings.Host}/users", user.Map());
        }
    }
}
