using BerghAdmin.ApplicationServices.KentaaInterface;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace BerghAdmin.Tests.Kentaa;


// Test only works with test site from Kentaa 
// 
[TestFixture]
public class InterfaceTests : DatabasedTests
{
    protected override void RegisterServices(ServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Services.Configuration.MailJetConfiguration>()
            .Build();

        services.AddHttpClient()
            .Configure<KentaaConfiguration>(configuration.GetSection("KentaaConfiguration"))
            .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>();
    }

    [Test]
    public async Task GetKentaDonationById()
    { 
        var service = this.GetRequiredService<IKentaaInterfaceService>();

        var kentaaDonation = await service.GetDonationById(2587623);
    
        Assert.IsNotNull(kentaaDonation);
        Assert.AreEqual("Floris", kentaaDonation.FirstName);
        Assert.AreEqual("Zwarteveen", kentaaDonation.LastName);   
    }

    [Test]
    public async Task GetKentaaDonationsPer4()
    {
        var service = this.GetRequiredService<IKentaaInterfaceService>();
        var filter = new KentaaFilter()
        {
            StartAt = 1,
            PageSize = 4
        };
        var kentaaDonations = await service.GetKentaaIssuesByQuery<KM.Donations, KM.Donation>(filter);

        Assert.IsTrue(kentaaDonations.Count() > 0);
    }

    [Test]
    public async Task GetKentaaDonationsPer25()
    {
        var service = this.GetRequiredService<IKentaaInterfaceService>();
        var filter = new KentaaFilter()
        {
            StartAt = 1,
            PageSize = 25
        };
        var kentaaDonations = await service.GetKentaaIssuesByQuery<KM.Donations, KM.Donation>(filter);

        Assert.IsTrue(kentaaDonations.Count() > 0);
    }

    [Test]
    public async Task GetKentaaUsersPer25()
    {
        var service = this.GetRequiredService<IKentaaInterfaceService>();
        var filter = new KentaaFilter()
        {
            StartAt = 1,
            PageSize = 25
        };
        var kentaaUsers = await service.GetKentaaIssuesByQuery<KM.Users, KM.User>(filter);

        Assert.IsTrue(kentaaUsers.Count() > 0);
    }

    [Test]
    public async Task GetKentaaActionsPer25()
    {
        var service = this.GetRequiredService<IKentaaInterfaceService>();
        var filter = new KentaaFilter()
        {
            StartAt = 1,
            PageSize = 25
        };
        var kentaaActions = await service.GetKentaaIssuesByQuery<KM.Actions, KM.Action>(filter);

        Assert.IsTrue(kentaaActions.Count() > 0);
    }

    [Test]
    public async Task GetKentaaProjectsPer25()
    {
        var service = this.GetRequiredService<IKentaaInterfaceService>();
        var filter = new KentaaFilter()
        {
            StartAt = 1,
            PageSize = 25
        };
        var kentaaProjects = await service.GetKentaaIssuesByQuery<KM.Projects, KM.Project>(filter);

        Assert.IsTrue(kentaaProjects.Count() > 0);
    }

    [Test]
    public async Task GetKentaaDonationsOnDate20220116()
    {
        var service = this.GetRequiredService<IKentaaInterfaceService>();
        var filter = new KentaaFilter()
        {
            StartAt = 1,
            PageSize = 25,
            CreatedAfter = new DateTime(2022, 1, 16),
            CreatedBefore = new DateTime(2022, 1, 17)
        };
        var kentaaDonations = await service.GetKentaaIssuesByQuery<KM.Donations, KM.Donation>(filter);

        Assert.IsTrue(kentaaDonations.Count() == 8);
    }

    [Test]
    public async Task GetKentaaDonationsBetweenDates()
    {
        var service = this.GetRequiredService<IKentaaInterfaceService>();
        var filter = new KentaaFilter()
        {
            StartAt = 1,
            PageSize = 25,
            CreatedAfter = new DateTime(2021, 12, 6),
            CreatedBefore = new DateTime(2022, 1, 17)
        };
        var kentaaDonations = await service.GetKentaaIssuesByQuery<KM.Donations, KM.Donation>(filter);

        Assert.IsTrue(kentaaDonations.Count() == 11);
    }
}
