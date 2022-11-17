using Microsoft.Playwright.NUnit;
using NUnit.Framework;

using System.Net;

namespace BerghAdmin.UITests;

[TestFixture]
public class LoginTests:PageTest
{
    public LoginTests():base()
    {

    }
    [Test]

    public async Task LoginPageLoads()
    {
        var page = await Page.GotoAsync("https://bergh-test-admin-webapp.azurewebsites.net/Identity/Account/Login?ReturnUrl=%2F");
        
        Assert.NotNull(page);
        Assert.IsTrue(page!.Ok);
        Assert.AreEqual((int)HttpStatusCode.OK, page!.Status);
    }
}