using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Presyotect.Core.Types;

namespace Presyotect.Features.Users;

public class ApiKeyAuthenticator(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    IConfiguration configuration,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    internal const string AuthenticationScheme = "ApiKey";
    internal const string HeaderName = "x-api-key";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(HeaderName, out var extractedApiKey);

        var apiKey = extractedApiKey;

        if (string.IsNullOrEmpty(apiKey))
        {
            return Task.FromResult(AuthenticateResult.Fail("Api key is not set."));
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Task.FromResult(AuthenticateResult.Fail("Provided api key is invalid."));
        }

        if (!string.Equals(apiKey, configuration["SuperAdmin:Key"]))
        {
            return Task.FromResult(AuthenticateResult.Fail("Provided api key is invalid."));
        }
        
        var identity = new ClaimsIdentity(
            claims: [new Claim("ClientID", "sa")],
            authenticationType: Scheme.Name);

        var principal = new GenericPrincipal(identity, [Role.SuperAdmin]);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));

    }
}