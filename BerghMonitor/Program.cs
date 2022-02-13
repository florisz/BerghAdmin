using System.Net;

await Check("BerghAdmin", "https://localhost:5001/health");
await Check("Kentaa API", "https://api.kentaa.nl/v1/actions?api_key=12345", HttpStatusCode.Unauthorized);
await Check("Kentaa Interface Function", "http://localhost:7071/api/health");


async Task Check(string name, string endpoint, HttpStatusCode validStatusCode = HttpStatusCode.OK)
{
    var client = new HttpClient();
    try
    {
        var response = await client.GetAsync(endpoint);
        if (response.StatusCode == validStatusCode)
        {
            WriteSuccess(name);
        }
        else
        {
            WriteFailure(name);
        }
    }
    catch (HttpRequestException ex)
    {
        WriteFailure($"{name}: {ex.Message}");
    }
}

void WriteSuccess(string message)
{
    var color = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(message);
    Console.ForegroundColor = color;
}

void WriteFailure(string message)
{
    var color = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
    Console.ForegroundColor = color;
}
