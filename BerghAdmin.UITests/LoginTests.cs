using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

using System.Net;
using System.Text.RegularExpressions;

namespace BerghAdmin.UITests;

[TestFixture]
public class LoginTests : PageTest
{
    public LoginTests() : base()
    {

    }
    
    [Test]
    public async Task LoginPageLoads()
    {
        var page = await Page.GotoAsync("https://bergh-test-admin-webapp.azurewebsites.net/Identity/Account/Login?ReturnUrl=%2F");

        Assert.That(page, !Is.EqualTo(null));
        Assert.That(page!.Ok == true);
        Assert.That((int)HttpStatusCode.OK == page!.Status);
    }

    [Test]
    public async Task FietsersPageShowsData()
    {
        await Page.GotoAsync("https://bergh-test-admin-webapp.azurewebsites.net/Identity/Account/Login?ReturnUrl=%2F");
        await Page.GetByLabel("E-mail").ClickAsync();
        await Page.GetByLabel("E-mail").FillAsync("admin@berghinhetzadel.nl");
        await Page.GetByLabel("E-mail").PressAsync("Tab");
        await Page.GetByLabel("Wachtwoord").FillAsync("Qwerty@123");
        await Page.GetByLabel("Wachtwoord").PressAsync("Enter");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Beheer Fietsers" }).ClickAsync();

        // Find title
        var title = Page.GetByText("Fietstochten", new() { Exact = true });
    }
}
