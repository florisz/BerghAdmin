using BerghAdmin.Services.Donaties;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace BerghAdmin.Tests.DonatieTests;

[TestFixture]
public class KentaaTests : DatabasedTests
{
    protected override void RegisterServices(ServiceCollection services)
    {
        services
            .AddScoped<IKentaaService, KentaaService>()
        ;
    }

    [Test]
    public void GetAll()
    {
        Assert.Fail();
    }

}
