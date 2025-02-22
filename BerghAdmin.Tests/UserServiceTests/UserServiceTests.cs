using BerghAdmin.Data;
using BerghAdmin.Authorization;
using BerghAdmin.Services.UserManagement;
using BerghAdmin.DbContexts;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System.Security.Claims;

using NUnit.Framework;

namespace BerghAdmin.Tests.UserServiceTests;

[TestFixture]
public class UserServiceTests : DatabaseTestSetup
{
    private const string DEFAULT_PASSWORD = "Qwerty@123";

    protected override void RegisterServices(ServiceCollection services)
    {
        services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthorizationCore();
        services.AddAuthenticationCore();
        services.AddAuthorizationPolicyEvaluator();
        services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services
            .AddScoped<IUserService, UserService>()
            .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        services.AddDataProtection();
    }

    [Test]
    public async Task AddUserWithoutClaimsTest()
    {
        var service = this.GetRequiredService<IUserService>();

        var userName = "PommetjeHorlepiep";
        var newUser = CreateUser(userName);
        var result = await service.InsertUserAsync(newUser, DEFAULT_PASSWORD);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }

        var user = await service.GetUserAsync(userName);
        Assert.That(user, !Is.EqualTo(null)); 
        Assert.That(userName == user!.UserName);
    }

    [Test]
    public async Task AddUserWithOneClaimTest()
    {
        var service = this.GetRequiredService<IUserService>();

        var claims = new Claim[] { AdministratorPolicyHandler.Claim };
        var userName = "PommetjeHorlepiep";
        var newUser = CreateUser(userName);
        var result = await service.InsertUserAsync(newUser, DEFAULT_PASSWORD, claims);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }

        var user = await service.GetUserAsync(userName);
        Assert.That(user, !Is.EqualTo(null));

        var userClaims = await service.GetUserClaimsAsync(userName);
        Assert.That(1 == userClaims.Count);
        Assert.That("role" == userClaims[0].Type);
        Assert.That("administrator" == userClaims[0].Value);
    }

    [Test]
    public async Task AddUserWithTwoClaimsTest()
    {
        var service = this.GetRequiredService<IUserService>();

        var claims = new Claim[] { AdministratorPolicyHandler.Claim, BeheerSecretariaatPolicyHandler.Claim };
        var userName = "PommetjeHorlepiep";
        var newUser = CreateUser(userName);
        var result = await service.InsertUserAsync(newUser, DEFAULT_PASSWORD, claims);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }

        var user = await service.GetUserAsync(userName);
        Assert.That(user, !Is.EqualTo(null));

        var userClaims = await service.GetUserClaimsAsync(userName);
        Assert.That(2 == userClaims.Count);
        Assert.That("role" == userClaims[0].Type);
        Assert.That("administrator" == userClaims[0].Value);
        Assert.That("role" == userClaims[1].Type);
        Assert.That("beheersecretariaat" == userClaims[1].Value);
    }

    [Test]
    public async Task AddAndGetMultipleUsers()
    {
        var service = this.GetRequiredService<IUserService>();

        var userName = "PommetjeHorlepiep";
        for (var i = 1; i <= 5; i++)
        {
            var newUser = CreateUser($"{userName}{i}");
            var result = await service.InsertUserAsync(newUser, DEFAULT_PASSWORD);
            if (!result.Succeeded)
            {
                Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
            }
        }

        var users = service.GetUsers();
        Assert.That(users, !Is.EqualTo(null));
        Assert.That(5 == users.Count);
        Assert.That(users.All(u => u.UserName != null && u.UserName.StartsWith(userName)) == true);
    }

    [Test]
    public async Task UpdatePropertyOfUser()
    {
        var service = this.GetRequiredService<IUserService>();

        var userName = "PommetjeHorlepiep";
        var newUser = CreateUser(userName);
        var result = await service.InsertUserAsync(newUser, DEFAULT_PASSWORD);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }

        var user = await service.GetUserAsync(userName);
        user!.PhoneNumber = "06-12345678";
        result = await service.UpdateUserAsync(user);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }
        var updatedUser = await service.GetUserAsync(userName);
        Assert.That(updatedUser, !Is.EqualTo(null));
        Assert.That(updatedUser!.PhoneNumber == "06-12345678");
    }

    [Test]
    public async Task UpdatePasswordOfUser()
    {
        var service = this.GetRequiredService<IUserService>();

        var userName = "PommetjeHorlepiep";
        var newUser = CreateUser(userName);
        var result = await service.InsertUserAsync(newUser, DEFAULT_PASSWORD);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }

        var user = await service.GetUserAsync(userName);
        Assert.That(user, !Is.EqualTo(null));

        var newPassword = "NewP@ssword#123";
        result = await service.UpdateUserPasswordAsync(user!, newPassword);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }

        var updatedUser = await service.GetUserAsync(userName);
        Assert.That(updatedUser, !Is.EqualTo(null));
        Assert.That(string.IsNullOrEmpty(updatedUser!.PasswordHash) == false);
    }

    [Test]
    public async Task UpdateClaimsOfUser()
    {
        var service = this.GetRequiredService<IUserService>();

        var claims = new Claim[] { AdministratorPolicyHandler.Claim };
        var userName = "PommetjeHorlepiep";
        var newUser = CreateUser(userName);
        var result = await service.InsertUserAsync(newUser, DEFAULT_PASSWORD, claims);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }

        var user = await service.GetUserAsync(userName);
        Assert.That(user, !Is.EqualTo(null));

        var newClaims = new Claim[] { BeheerSecretariaatPolicyHandler.Claim, BeheerGolfersPolicyHandler.Claim };
        result = await service.UpdateUserAsync(user!, newClaims);
        if (!result.Succeeded)
        {
            Assert.Fail(string.Join(",", result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
        }

        var updatedUser = await service.GetUserAsync(userName);
        Assert.That(updatedUser, !Is.EqualTo(null));

        var updatedClaims = await service.GetUserClaimsAsync(userName);
        Assert.That(2 == updatedClaims.Count);
        Assert.That("role" == updatedClaims[0].Type);
        Assert.That("beheersecretariaat" == updatedClaims[0].Value);
        Assert.That("role" == updatedClaims[1].Type);
        Assert.That("beheergolfers" == updatedClaims[1].Value);
    }

    private User CreateUser(string naam)
    {
        var user = new User
        {
            Name = naam,
            UserName = naam,
            Email = $"{naam}@berghinhetzadel.nl",
            AccessFailedCount = 0,
            EmailConfirmed = true,
            LockoutEnabled = false,
            LockoutEnd = null,
            PhoneNumber = "",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
        };

        return user;
    }
}
