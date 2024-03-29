using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BerghAdmin.EFDataTests;

// !!! IMPORTANT !!!
// this solution is quick hack so it works for me (Floris)
// I only used these tests to test multi threading issues with EF Core
// I did not use these tests to test the actual functionality of the application
// so I did not bother to setup the services and context properly
// I just copied the code from the application and made it work for me
// I did not bother to make it work for you
// !!! IMPORTANT !!!
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
public class MultiThreadingTests : SetupServicesAndContext
{

    protected override void RegisterDatabaseAccess(IServiceCollection services)
    {
        var version = ServerVersion.Parse("5.7");
        var connectionString = "Server=localhost;User=BerghAdmin;Password=qwerty@123;Database=BIHZ2021";
        Action<MySqlDbContextOptionsBuilder> option = o => o.EnableRetryOnFailure();
        services
           .AddDbContext<ApplicationDbContext>(
                options => options.UseMySql(connectionString, version, option))
           .AddDbContextFactory<ApplicationDbContext>(
                    options => options
                            .UseMySql(connectionString, version, option)
                            .EnableSensitiveDataLogging(true)
                            .EnableDetailedErrors(true),
                    ServiceLifetime.Scoped);
    }

    protected override void RegisterServices(ServiceCollection services)
    {
        services.AddScoped<IPersoonService, PersoonService>();
        services.AddScoped<IFietstochtenService, FietstochtenService>();
        services.AddScoped<IRolService, RolService>();
        services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
    }

    // Multithreading tests are NOT necessary anymore, maybe later
    // If reinstated: check code carefully first!

    //[Test]
    //public async Task TestPersoonCanBeUpdatedInMoreScopes()
    //{
    //    // RESULT: this works fine because the test runs one one thread
    //    //         the updates are done sequentially and independently from each other (as expected)
    //    //
    //    // create a new service lifetime scope
    //    // this is necessary because the service provider is disposed after each test
    //    // and we need to create a new service provider parallel to the existing one
    //    // so we can test the multi threading issues
    //    using var scope = this.GetRequiredService<IServiceScopeFactory>().CreateScope();
    //    var serviceProvider1 = this.GetRequiredService<IServiceProvider>();
    //    var serviceProvider2 = scope.ServiceProvider;

    //    const string newAchternaam1 = "Apenootzzzz";
    //    const string newAchternaam2 = "Bengelzzzzz";

    //    // Persoon on thread 1
    //    var persoonService1 = serviceProvider1.GetRequiredService<IPersoonService>();
    //    var persoon1 = await persoonService1.GetByEmailAdres("appie@aapnootmies.com");
    //    Assert.NotNull(persoon1);
    //    Assert.AreEqual(persoon1.Voornaam, "Appie");
    //    persoon1.Achternaam = newAchternaam1;

    //    // Persoon on thread 2
    //    var persoonService2 = serviceProvider2.GetRequiredService<IPersoonService>();
    //    var persoon2 = await persoonService2.GetByEmailAdres("bert@aapnootmies.com");
    //    Assert.NotNull(persoon2);
    //    Assert.AreEqual(persoon2.Voornaam, "Bert");
    //    persoon2.Achternaam = newAchternaam2;

    //    try
    //    {
    //        await persoonService1.SavePersoonAsync(persoon1);
    //        persoon1 = await persoonService1.GetByEmailAdres("appie@aapnootmies.com");
    //        Assert.AreEqual(persoon1.Achternaam, newAchternaam1);

    //        await persoonService2.SavePersoonAsync(persoon2);
    //        persoon2 = await persoonService2.GetByEmailAdres("bert@aapnootmies.com");
    //        Assert.AreEqual(persoon2.Achternaam, newAchternaam2);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }

    //}

    //// Error: System.InvalidOperationException:
    //// A second operation was started on this context instance before a previous operation completed.
    //// This is usually caused by different threads concurrently using the same instance of DbContext.
    //// For more information on how to avoid threading issues with DbContext, see...
    //[Test]
    //public async Task TestUseTrackedAndUntrackedDataOn2ThreadsBut1Scope()
    //{
    //    // RESULT: this works fine because the context is the same for both threads

    //    // read untracked data first and then update one of the elements
    //    using var scope = this.GetRequiredService<IServiceScopeFactory>().CreateScope();
    //    var serviceProvider = scope.ServiceProvider;

    //    const string newAchternaam1 = "Apenootxxx";

    //    // On the update thread we will try to update the data
    //    var updateThread = new Thread(async() =>
    //    {
    //        Thread.Sleep(1000);
    //        var persoonService1 = this.GetRequiredService<IPersoonService>();

    //        // and try to save this person
    //        try
    //        {
    //            // now edit one person
    //            var persoon = await persoonService1.GetByEmailAdres("appie@aapnootmies.com");
    //            Assert.NotNull(persoon);
    //            Assert.AreEqual(persoon.Voornaam, "Appie");
    //            persoon.Achternaam = newAchternaam1;

    //            await persoonService1.SavePersoonAsync(persoon);
    //            persoon = await persoonService1.GetByEmailAdres("appie@aapnootmies.com");
    //            Assert.AreEqual(persoon.Achternaam, newAchternaam1);
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    });

    //    // on the read thread the data will be read
    //    var readThread = new Thread(() =>
    //    {
    //        var persoonService2 = this.GetRequiredService<IPersoonService>();

    //        try
    //        {
    //            // first read all persons into a list including rollen and fietstochten
    //            // data is untracked (see PersoonService, temp change)
    //            var personen = persoonService2.GetPersonen();
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //        // wait for thread 1 for 10 minutes to do the update
    //        updateThread.Join(new TimeSpan(0,10,0));
    //    });

    //    // start the read thread first
    //    updateThread.Start();
    //    // wait a while so the read has been done
    //    Thread.Sleep(1000);
    //    // start the update thread
    //    readThread.Start();
    //    // wait for both threads to finish
    //    readThread.Join();
    //    updateThread.Join();
    //}

    //[Test]
    //public void TestUseTrackedAndUntrackedDataOn2ThreadsAnd2Contexts()
    //{
    //    // create a new service lifetime scope and split the read and update threads
    //    // this is necessary because the service provider is disposed after each test
    //    // and we need to create a new service provider parallel to the existing one
    //    // so we can test the multi threading issues
    //    using var scope = this.GetRequiredService<IServiceScopeFactory>().CreateScope();
    //    var serviceProvider1 = this.GetRequiredService<IServiceProvider>();
    //    var serviceProvider2 = scope.ServiceProvider;

    //    const string newAchternaam = "Apenootyyy";

    //    // On the update thread we will try to update the data
    //    var updateThread = new Thread(async() =>
    //    {
    //        Thread.Sleep(1000);
    //        // read untracked data first and then update one of the elements
    //        var persoonService1 = serviceProvider1.GetRequiredService<IPersoonService>();

    //        // and try to save this person
    //        try
    //        {
    //            // now edit one person
    //            var persoon = await persoonService1.GetByEmailAdres("appie@aapnootmies.com");
    //            Assert.NotNull(persoon);
    //            Assert.AreEqual(persoon?.Voornaam, "Appie");
    //            if (persoon != null)
    //            {
    //                persoon.Achternaam = newAchternaam;
    //            }

    //            await persoonService1.SavePersoonAsync(persoon);
    //            persoon = await persoonService1.GetByEmailAdres("appie@aapnootmies.com");
    //            Assert.AreEqual(persoon?.Achternaam, newAchternaam);
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    });

    //    // on the read thread the data will be read
    //    var readThread = new Thread(() =>
    //    {
    //        var persoonService2 = serviceProvider2.GetRequiredService<IPersoonService>();

    //        // first read all persons into a list including rollen and fietstochten
    //        // data is untracked (see PersoonService, temp change)
    //        var personen = persoonService2.GetPersonen();

    //        // wait for thread 1 for 10 minutes to do the update
    //        updateThread.Join(new TimeSpan(0, 10, 0));
    //    });

    //    // start the update thread
    //    updateThread.Start();
    //    // start the read thread first
    //    readThread.Start();
    //    // wait a while so the read has been done
    //    Thread.Sleep(1000);
    //    // wait for both threads to finish
    //    readThread.Join();
    //    updateThread.Join();
    //}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.

}
