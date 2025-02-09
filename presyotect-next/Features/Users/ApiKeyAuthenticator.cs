using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Presyotect.Features.Users;

public class ApiKeyAuthenticator(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    internal const string AuthenticationScheme = "ApiKey";
    internal const string HeaderName = "x-api-key";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(HeaderName, out var extractedApiKey);

        var apiKey = extractedApiKey;

        if (string.IsNullOrEmpty(apiKey))
        {
            return AuthenticateResult.Fail("Api key is not set.");
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return AuthenticateResult.Fail("Provided api key is invalid.");
        }

        var identity = new ClaimsIdentity(
            claims: [new Claim("ClientID", "dev")],
            authenticationType: Scheme.Name);

        var principal = new GenericPrincipal(identity, roles: ["Admin"]);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}