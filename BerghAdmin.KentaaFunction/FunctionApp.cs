using BerghAdmin.ApplicationServices.KentaaInterface;
using BerghAdmin.KentaaFunction.Services;

using Microsoft.Azure.WebJobs;

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
    public async Task ReadUsers([TimerTrigger("0 0 1 * * *", RunOnStartup = false)] TimerInfo myTimer)
    {
        var users = kentaaService.GetKentaaResourcesByQuery<KM.Users, KM.User>(new KentaaFilter());
        await berghService.Send<KM.User>(users);
    }

    [FunctionName(nameof(ReadProjects))]
    public async Task ReadProjects([TimerTrigger("0 0 2 * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        var projects = kentaaService.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(new KentaaFilter());
        await berghService.Send<KM.Project>(projects);
    }

    [FunctionName(nameof(ReadActions))]
    public async Task ReadActions([TimerTrigger("0 0 3 * * *", RunOnStartup = false)] TimerInfo myTimer)
    {
        var actions = kentaaService.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(new KentaaFilter());
        await berghService.Send<KM.Action>(actions);
    }

    [FunctionName(nameof(ReadDonations))]
    public async Task ReadDonations([TimerTrigger("0 0 4 * * *", RunOnStartup = false)] TimerInfo myTimer)
    {
        var donaties = kentaaService.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(new KentaaFilter());
        await berghService.Send<KM.Donation>(donaties);
    }
}
