using BerghAdmin.ApplicationServices.KentaaInterface;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;

namespace BerghAdmin.KentaaFunction;
public class FunctionApp
{
    private readonly IKentaaInterfaceService service;
    private readonly HttpClient berghClient;

    public FunctionApp(IKentaaInterfaceService service, HttpClient berghClient)
    {
        this.service = service;
        this.berghClient = berghClient;
    }

    [FunctionName("ReadDonations")]
    public async Task ReadDonations([TimerTrigger("0/10 * * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        var donaties = await service.GetDonationsByQuery(new KentaaFilter());
        foreach (var donatie in donaties)
        {
            await berghClient.PostAsJsonAsync("", donatie);
        }
    }
}
