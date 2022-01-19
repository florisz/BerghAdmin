using BerghAdmin.ApplicationServices.KentaaInterface;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;

namespace BerghAdmin.KentaaFunction;
public class FunctionApp
{
    private readonly IKentaaInterfaceService service;

    public FunctionApp(IKentaaInterfaceService service)
    {
        this.service = service;
    }

    [FunctionName("ReadDonations")]
    public void ReadDonations([TimerTrigger("0/10 * * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        service.GetDonationsByQuery();
    }
}
