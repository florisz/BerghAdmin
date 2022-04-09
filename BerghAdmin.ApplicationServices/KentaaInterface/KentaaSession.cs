using System.Net.Http.Headers;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public class KentaaSession
{
    private readonly string _apiKey;
    private readonly string _kentaaHost;
    private readonly string _kentaaBasePath;
    private HttpClient? _httpClient = null;

    public KentaaSession(string kentaaHost, string kentaaBasePath, string apiKey)
    {
        _apiKey = apiKey;
        _kentaaHost = kentaaHost;
        _kentaaBasePath = kentaaBasePath;
    }

    public HttpClient Connect(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "BerghAdmin - Kentaa interface");
        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
        
        return _httpClient;
    }

    public string Url(string subPath)
    {
        return Url(subPath, null);
    }

    public string Url(string subPath, KentaaFilter? filter)
    {
        var path = $"{_kentaaBasePath}/{subPath}";
        var query = filter?.Build();
        
        UriBuilder builder = new("https", _kentaaHost);
        builder.Path = path;
        builder.Query = query;

        return builder.ToString();
    }
}
