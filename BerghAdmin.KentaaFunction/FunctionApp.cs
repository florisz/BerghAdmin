using BerghAdmin.ApplicationServices.KentaaInterface;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using System;

namespace BerghAdmin.KentaaFunction;
public class FunctionApp
{
    private readonly IKentaaInterfaceService service;
    private readonly HttpClient berghClient;

    public FunctionApp(IHttpClientFactory httpClientFactory, IKentaaInterfaceService service)
    {
        this.service = service;
        this.berghClient = httpClientFactory.CreateClient();
    }

    [FunctionName("ReadDonations")]
    public async Task ReadDonations([TimerTrigger("0/10 * * * * *")] TimerInfo myTimer, ILogger log)
    {
        var donaties = service.GetKentaaIssuesByQuery<KM.Donations,KM.Donation>(new KentaaFilter());
        await foreach (var donatie in donaties)
        {
            await berghClient.PostAsJsonAsync("https://localhost:5001/donaties", donatie);
        }
    }
}
