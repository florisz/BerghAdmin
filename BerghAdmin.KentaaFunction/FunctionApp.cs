using BerghAdmin.ApplicationServices.KentaaInterface;
using BerghAdmin.KentaaFunction.Services;

using HealthChecks.UI.Core;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.KentaaFunction;
public class FunctionApp
{
    private readonly IKentaaInterfaceService kentaaService;
    private readonly BerghAdminService berghService;

    public FunctionApp(IKentaaInterfaceService kentaaService, BerghAdminService berghService)
    {
        this.kentaaService = kentaaService;
        this.berghService = berghService;
    }

    [FunctionName(nameof(ReadUsers))]
    public async Task ReadUsers([TimerTrigger("%cron_users%", RunOnStartup = false)] TimerInfo myTimer)
    {
        var users = kentaaService.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter());
        await berghService.Send<KM.User>(users);
    }

    [FunctionName(nameof(ReadProjects))]
    public async Task ReadProjects([TimerTrigger("%cron_projects%", RunOnStartup = true)] TimerInfo myTimer)
    {
        var projects = kentaaService.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
        await berghService.Send<KM.Project>(projects);
    }

    [FunctionName(nameof(ReadActions))]
    public async Task ReadActions([TimerTrigger("%cron_actions%", RunOnStartup = false)] TimerInfo myTimer)
    {
        var actions = kentaaService.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());
        await berghService.Send<KM.Action>(actions);
    }

    [FunctionName(nameof(ReadDonations))]
    public async Task ReadDonations([TimerTrigger("%cron_donations%", RunOnStartup = false)] TimerInfo myTimer)
    {
        var donaties = kentaaService.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
        await berghService.Send<KM.Donation>(donaties);
    }

    [FunctionName(nameof(Health))]
    public IActionResult Health([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        var now = DateTime.Now;
        return new OkObjectResult(
            new UIHealthReport(null, DateTime.Now.Subtract(now))
            {
                Status = UIHealthStatus.Healthy,
            }
        );
    }
}
