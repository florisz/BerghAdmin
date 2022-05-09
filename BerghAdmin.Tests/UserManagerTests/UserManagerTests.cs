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

namespace BerghAdmin.Tests.UserManagerTests
{
    [TestFixture]
    public class UserManagerTests : DatabasedTests
    {
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
            var errors = await service.InsertUserAsync(userName);
            if (errors != null)
            {
                Assert.Fail(string.Join(",", errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
            }

            var user = await service.GetUserAsync(userName);
            Assert.IsNotNull(user);
            Assert.AreEqual(userName, user.UserName);
        }

        [Test]
        public async Task AddUserWithoutClaimsWitPersoonTest()
        {
            var service = this.GetRequiredService<IUserService>();

            var userName = "PommetjeHorlepiep";
            var persoon = new Persoon() { Id = 42, Voornaam = "Pommetje", Achternaam = "Horlepiep" };
            var errors = await service.InsertUserAsync(userName, persoon);
            if (errors != null)
            {
                Assert.Fail(string.Join(",", errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
            }

            var user = await service.GetUserAsync(userName);
            Assert.IsNotNull(user);
            Assert.AreEqual(userName, user.UserName);
            Assert.AreEqual(42, user.CurrentPersoonId);
        }

        [Test]
        public async Task AddUserWithOneClaimTest()
        {
            var service = this.GetRequiredService<IUserService>();
            var claims = new Claim[] { AdministratorPolicyHandler.Claim };
            var userName = "PommetjeHorlepiep";
            var errors = await service.InsertUserAsync(userName, claims);
            if (errors != null)
            {
                Assert.Fail(string.Join(",", errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
            }

            var user = await service.GetUserAsync(userName);
            Assert.IsNotNull(user);

            var userClaims = await service.GetUserClaimsAsync(userName);
            Assert.AreEqual(1, userClaims.Count);
        }

        [Test]
        public async Task AddUserWithTwoClaimsTest()
        {
            var service = this.GetRequiredService<IUserService>();
            var claims = new Claim[] { AdministratorPolicyHandler.Claim, BeheerSecretariaatPolicyHandler.Claim };
            var userName = "PommetjeHorlepiep";
            var errors = await service.InsertUserAsync(userName, claims);
            if (errors != null)
            {
                Assert.Fail(string.Join(",", errors.Select(e => $"{e.Code}: {e.Description}").ToArray()));
            }

            var user = await service.GetUserAsync(userName);
            Assert.IsNotNull(user);

            var userClaims = await service.GetUserClaimsAsync(userName);
            Assert.AreEqual(2, userClaims.Count);
        }
    }
}
