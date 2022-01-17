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

        services.AddScoped<IKentaaInterfaceService, KentaaInterfaceService>();
        services.Configure<KentaaConfiguration>(configuration.GetSection("KentaaConfiguration"));
        services.AddScoped<IKentaaService, KentaaService>();
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
    public async Task GetKentaaDonations()
    {
        var service = this.GetRequiredService<IKentaaInterfaceService>();
        var filter = new KentaaFilter()
        {
            StartAt = 1,
            PageSize = 4
        };
        var kentaaDonations = await service.GetDonationsByQuery(filter);

        Assert.IsTrue(kentaaDonations.Count() >= 13);
    }

}
