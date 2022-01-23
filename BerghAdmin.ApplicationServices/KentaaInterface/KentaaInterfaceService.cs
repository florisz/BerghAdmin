using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using Microsoft.Extensions.Options;

using System.Net.Http;
using System.Text.Json;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public class KentaaInterfaceService : IKentaaInterfaceService
{
    readonly HttpClient _httpClient;
    readonly KentaaSession _session;
    public KentaaInterfaceService(IOptions<KentaaConfiguration> settings, IHttpClientFactory factory)
    {
        _session = new KentaaSession(settings.Value.KentaaHost, settings.Value.KentaaBasePath, settings.Value.ApiKey);
        _httpClient = _session.Connect(factory);
    }

    public async Task<IEnumerable<KentaaModel.Action>> GetActionsByQuery(KentaaFilter filter)
    {
        var actions = new List<KentaaModel.Action>();

        var url = _session.Url("actions", filter);

        var response = await GetKentaaResponse<Actions>(url);

        while (response.ActionArray != null && response.ActionArray.Any())
        {
            actions.AddRange(response.ActionArray);

            filter = filter.NextPage();
            url = _session.Url("actions", filter);
            response = await GetKentaaResponse<Actions>(url);
        }

        return actions;
    }

    public async Task<Donation> GetDonationById(int donationId)
    {
        var url = _session.Url($"donations/{donationId}");
        var donation = await GetKentaaResponse<DonationResponse>(url);
        return donation.data;
    }

    public async Task<IEnumerable<Donation>> GetDonationsByQuery(KentaaFilter filter)
    {
        var donations = new List<Donation>();

        var url = _session.Url("donations", filter);
        var response = await GetKentaaResponse<Donations>(url);

        while (response.DonationArray != null && response.DonationArray.Any())
        {
            donations.AddRange(response.DonationArray);

            filter = filter.NextPage();
            url = _session.Url("donations", filter);
            response = await GetKentaaResponse<Donations>(url);
        }

        return donations;
    }

    public async Task<IEnumerable<Project>> GetProjectsByQuery(KentaaFilter filter)
    {
        var projects = new List<Project>();

        var url = _session.Url("projects", filter);
        var response = await GetKentaaResponse<Projects>(url);

        while (response.ProjectArray != null && response.ProjectArray.Any())
        {
            projects.AddRange(response.ProjectArray);

            filter = filter.NextPage();
            url = _session.Url("projects", filter);
            response = await GetKentaaResponse<Projects>(url);
        }

        return projects;
    }

    public async Task<IEnumerable<User>> GetUsersByQuery(KentaaFilter filter)
    {
        var users = new List<User>();

        var url = _session.Url("users", filter);
        var response = await GetKentaaResponse<Users>(url);

        while (response.UserArray != null && response.UserArray.Any())
        {
            users.AddRange(response.UserArray);

            filter = filter.NextPage();
            url = _session.Url("users", filter);
            response = await GetKentaaResponse<Users>(url);
        }

        return users;
    }

    private async Task<T> GetKentaaResponse<T>(string url)
    {
        var options = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var o = JsonSerializer.Deserialize<T>(json, options);
            if (o == null)
                throw new ApplicationException($"Could not deserialize JSON '{o}' into kentaa issue list");

            return o;
        }
    
        throw new ApplicationException($"Could not get {typeof(T).Name} donation from Kentaa; {url}");
    }

    private Uri BuildUri()
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Scheme = "https";
        uriBuilder.Host = "cnn.com";
        uriBuilder.Path = "americas";
        
        return uriBuilder.Uri;
    }
}