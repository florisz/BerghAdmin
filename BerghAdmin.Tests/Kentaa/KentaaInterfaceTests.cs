using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.KentaaInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace BerghAdmin.Tests.Kentaa;

[TestFixture]
public class KentaaInterfaceTests : DatabasedTests
{
    protected override void RegisterServices(ServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<KentaaConfiguration>()
            .Build();

        services.AddHttpClient()
            .AddScoped<IKentaaInterfaceService, KentaaInterfaceService>()
            .Configure<KentaaConfiguration>(configuration.GetSection("KentaaConfiguration"))
            .AddScoped<IKentaaService, KentaaService>();
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
        var kentaaDonations = await service.GetDonationsByQuery(filter);

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
        var kentaaDonations = await service.GetDonationsByQuery(filter);

        Assert.IsTrue(kentaaDonations.Count() > 0);
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
        var kentaaDonations = await service.GetDonationsByQuery(filter);

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
        var kentaaDonations = await service.GetDonationsByQuery(filter);

        Assert.IsTrue(kentaaDonations.Count() == 11);
    }
}
