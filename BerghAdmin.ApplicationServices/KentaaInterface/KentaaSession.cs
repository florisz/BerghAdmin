using System.Net.Http.Headers;

namespace BerghAdmin.Services.KentaaInterface;

public class KentaaSession
{
    private readonly string _apiKey;
    private readonly string _jiraServerUrl;
    private HttpClient? _httpClient = null;

    public KentaaSession(string jiraServerUrl, string apiKey)
    {
        _apiKey = apiKey;
        _jiraServerUrl = jiraServerUrl;
    }

    public string Url => _jiraServerUrl;

    public string ApiKey => _apiKey;

    public HttpClient Connect(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "BerghAdmin - Kentaa interface");

        return _httpClient;
    }
}
