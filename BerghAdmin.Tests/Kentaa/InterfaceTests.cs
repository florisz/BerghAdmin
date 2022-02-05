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
            .AddUserSecrets<KentaaConfiguration>()
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
        Assert.AreEqual("Floris", kentaaDonation.first_name);
        Assert.AreEqual("Zwarteveen", kentaaDonation.last_name);   
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
        var kentaaDonations = service.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(filter);

        Assert.IsTrue(await kentaaDonations.AnyAsync());
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
        var kentaaDonations = service.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(filter);

        Assert.IsTrue(await kentaaDonations.AnyAsync());
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
        var kentaaUsers = service.GetKentaaResourcesByQuery<KM.Users, KM.User>(filter);

        Assert.IsTrue(await kentaaUsers.AnyAsync());
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
        var kentaaActions = service.GetKentaaResourcesByQuery<KM.Actions, KM.Action>(filter);

        Assert.IsTrue(await kentaaActions.AnyAsync());
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
        var kentaaProjects = service.GetKentaaResourcesByQuery<KM.Projects, KM.Project>(filter);

        Assert.IsTrue(await kentaaProjects.AnyAsync());
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
        var kentaaDonations = service.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(filter);

        Assert.IsTrue(await kentaaDonations.CountAsync() == 8);
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
        var kentaaDonations = service.GetKentaaResourcesByQuery<KM.Donations, KM.Donation>(filter);

        Assert.IsTrue(await kentaaDonations.CountAsync() == 11);
    }
}
