using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using Microsoft.Extensions.Options;

using System.Text.Json;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public class KentaaInterfaceService : IKentaaInterfaceService
{
    readonly HttpClient _httpClient;
    readonly KentaaSession _session;
    readonly Dictionary<Type, string> endpoints = new(){
        { typeof(KM.Actions), "actions"},
        { typeof(KM.Projects), "projects"},
        { typeof(KM.Donations), "donations"},
        { typeof(KM.Users), "users"},
    };

    public KentaaInterfaceService(IOptions<KentaaConfiguration> settings, IHttpClientFactory factory)
    {
        if (settings == null || 
            string.IsNullOrEmpty(settings.Value.KentaaHost) ||
            string.IsNullOrEmpty(settings.Value.ApiKey))
        {
            throw new ApplicationException("Kentaa configuration is not containing correct values");
        }

        _session = new KentaaSession(settings.Value.KentaaHost, settings.Value.KentaaBasePath, settings.Value.ApiKey);
        _httpClient = _session.Connect(factory);
    }

    public async Task<Donation> GetDonationById(int donationId)
    {
        var url = _session.Url($"donations/{donationId}");
        var donation = await GetKentaaResponse<DonationResponse>(url);

        return donation.donation;
    }

    public async IAsyncEnumerable<T> GetKentaaResourcesByQuery<TList, T>(KentaaFilter filter) 
        where TList: IResources<T>
        where T: IResource
    {
        var endpoint = endpoints[typeof(TList)];
        var url = _session.Url(endpoint, filter);
        var resources = (await GetKentaaResponse<TList>(url)).GetResources();

        while (resources.Any())
        {
            foreach (var resource in resources)
            {
                yield return resource;
            }

            filter = filter.NextPage();
            url = _session.Url(endpoint, filter);
            resources = (await GetKentaaResponse<TList>(url)).GetResources();
        }
    }

    private async Task<T> GetKentaaResponse<T>(string url)
    {
        var options = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };
        try
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var o = JsonSerializer.Deserialize<T>(json, options);
                if (o == null)
                    throw new ApplicationException($"Could not deserialize JSON '{o}' into kentaa resource list");

                return o;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        throw new ApplicationException($"Could not get {typeof(T).Name} from Kentaa; {url}");
    }

}