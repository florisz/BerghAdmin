using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Data.Kentaa;

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

    public async Task Send(IAsyncEnumerable<IResource> resources)
    {
        await foreach (var resource in resources)
        {
            await berghClient.PostAsJsonAsync($"{settings.Host}/{resource.GetType().Name}", resource.Map());
        }
    }

    public IBihzResource Map(IResource resource)
    {
        return resource.Map();
    }
}
