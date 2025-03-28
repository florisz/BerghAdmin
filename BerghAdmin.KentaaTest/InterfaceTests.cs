﻿using BerghAdmin.ApplicationServices.KentaaInterface;
using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace BerghAdmin.Tests.Kentaa;


// Test only works with test site from Kentaa 
// 
[TestFixture]
public class InterfaceTests : DatabaseTestSetup
{
    protected override void RegisterServices(ServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<BerghAdmin.Program>()
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

        Assert.That(kentaaDonation, !Is.EqualTo(null));
        Assert.That("Floris" == kentaaDonation.first_name);
        Assert.That("Zwarteveen" == kentaaDonation.last_name);   
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

        Assert.That(await kentaaDonations.AnyAsync());
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

        Assert.That(await kentaaDonations.AnyAsync());
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

        Assert.That(await kentaaUsers.AnyAsync());
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

        Assert.That(await kentaaActions.AnyAsync());
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

        Assert.That(await kentaaProjects.AnyAsync());
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

        Assert.That(await kentaaDonations.CountAsync() == 8);
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

        Assert.That(await kentaaDonations.CountAsync() == 11);
    }
}
