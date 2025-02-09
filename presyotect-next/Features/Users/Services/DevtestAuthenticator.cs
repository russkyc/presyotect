using Presyotect.Features.Users.Models;
using Presyotect.Utilities;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Presyotect.Features.Users.Services;

public class DevtestAuthenticator(IConfiguration configuration) : IAuthenticator
{
    private static readonly ConcurrentDictionary<string, string> RefreshTokens = new();

    public Validation Validate(User user)
    {
        var validation = new Validation();
        string[] allowed = ["admin", "personnel"];
        if (allowed.Any(username => user.Username.Equals(username)))
        {
            validation.Valid = true;
            validation.Message = "User Valid";
            return validation;
        }

        validation.Message = "User does not exist.";
        return validation;
    }

    public string Tokenize(User user)
    {
        var refreshToken = TokenUtils.CreateRefreshToken();
        var token = TokenUtils.CreateToken(configuration, user.Username, refreshToken, user.Roles);
        RefreshTokens[user.Username] = refreshToken; // Store refresh token by username
        return token;
    }

    public Validation ValidateRefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var validation = new Validation();
        var principal = TokenUtils.GetPrincipalFromExpiredToken(configuration, refreshTokenRequest.Token);
        if (principal == null)
        {
            validation.Message = "Invalid token.";
            return validation;
        }

        var refreshToken = principal.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
        var username = principal.Claims.First(claim => claim.Type == "username").Value;
        if (RefreshTokens.TryGetValue(username, out var storedRefreshToken) &&
            storedRefreshToken == refreshToken)
        {
            validation.Valid = true;
            validation.Message = "Refresh token is valid.";
            return validation;
        }

        validation.Message = "Invalid refresh token.";
        return validation;
    }

    public Validation RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var isRequestValid = ValidateRefreshToken(refreshTokenRequest);
        var validation = new Validation();
        if (!isRequestValid.Valid)
        {
            validation.Message = "Token is invalid.";
            return validation;
        }

        var claims = TokenUtils.GetPrincipalFromExpiredToken(configuration, refreshTokenRequest.Token);

        if (claims is null)
        {
            validation.Message = "Token is invalid.";
            return validation;
        }
        
        var username = claims.Claims
            .First(claim => claim.Type.Equals("username"))
            .Value;
        var roles = claims.Claims
            .Where(claim => claim.Type.Equals(ClaimTypes.Role))
            .Select(claim => claim.Value)
            .ToArray();
        
        var newRefreshToken = TokenUtils.CreateRefreshToken();
        var newToken = TokenUtils.CreateToken(configuration, username, newRefreshToken, roles);
        
        RefreshTokens[username] = newRefreshToken; // Update refresh token by username
        RefreshTokens.TryRemove(refreshTokenRequest.Token, out _);

        validation.Valid = true;
        validation.Message = newToken;
        return validation;
    }
}