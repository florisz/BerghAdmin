using RestEase;

namespace BerghAdmin.Services.KentaaInterface;

public class KentaaSession
{
    string _apiKey;
    string _jiraServerUrl;

    public KentaaSession(string jiraServerUrl, string apiKey)
    {
        _apiKey = apiKey;
        _jiraServerUrl = jiraServerUrl;
    }

    public IKentaaClient Connect()
    {
        var client = RestClient.For<IKentaaClient>(_jiraServerUrl);

        return client;
    }
}
