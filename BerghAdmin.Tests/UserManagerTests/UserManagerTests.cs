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
        private UserManager<User> _userManager;

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
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services
                .AddScoped<IUserService, UserService>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));

            // just some testing
            _userManager = services.BuildServiceProvider().GetService<UserManager<User>>();
        }

        [Test]
        public async Task AddUserWithoutClaimsTest()
        {
            var service = this.GetRequiredService<IUserService>();
            var claims = new Claim[] { };
            var userName = "PommetjeHorlepiep";

            var user = service.GetUserAsync(userName);

            Assert.IsNotNull(user);
        }

        [Test]
        public async Task AddUserWithOneClaimTest()
        {
            var service = this.GetRequiredService<IUserService>();
            var claims = new Claim[] { AdministratorPolicyHandler.Claim };
            var userName = "PommetjeHorlepiep";
            await service.InsertUserAsync(userName, claims);

            var user = await service.GetUserAsync(userName);
            Assert.IsNotNull(user);

            var userClaims = await _userManager.GetClaimsAsync(user);
            Assert.AreEqual(1, userClaims.Count);
        }

        [Test]
        public async Task AddUserWithTwoClaimsTest()
        {
            var service = this.GetRequiredService<IUserService>();
            var claims = new Claim[] { AdministratorPolicyHandler.Claim, BeheerSecretariaatPolicyHandler.Claim };
            var userName = "PommetjeHorlepiep";
            await service.InsertUserAsync(userName, claims);

            var user = await service.GetUserAsync(userName);
            Assert.IsNotNull(user);

            var userClaims = await _userManager.GetClaimsAsync(user);
            Assert.AreEqual(2, userClaims.Count);
        }
    }
}
