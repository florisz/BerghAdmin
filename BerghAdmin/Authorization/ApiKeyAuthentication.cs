using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

using System.Text.Encodings.Web;

namespace BerghAdmin.Authorization
{
    public class ApiKeyAuthentication : AuthenticationHandler<ApiKeyOptions>
    {
        private const string APIKEYNAME = "api-key";

        public ApiKeyAuthentication(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;
            if (!headers.ContainsKey(APIKEYNAME))
                return Task.FromResult(AuthenticateResult.Fail("Api Key missing"));

            var apiKey = headers[APIKEYNAME];
            if (apiKey != "abcdefghijklm")
                return Task.FromResult(AuthenticateResult.Fail("Api Key incorrect"));

            return Task.FromResult(
                AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new System.Security.Claims.ClaimsPrincipal(),
                        "ApiKey"
                    )
                )
            );
        }
    }

    public class ApiKeyOptions:AuthenticationSchemeOptions
    {
    }
}
