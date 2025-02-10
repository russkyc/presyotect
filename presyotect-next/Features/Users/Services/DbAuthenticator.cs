using Presyotect.Features.Users.Models;
using Presyotect.Utilities;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LiteDB.Async;
using Presyotect.Core.Types;

namespace Presyotect.Features.Users.Services;

public class DbAuthenticator(IConfiguration configuration, ILiteDatabaseAsync database) : IAuthenticator
{
    private static readonly ConcurrentDictionary<string, string> RefreshTokens = new();

    public async Task<Validation> Validate(LoginRequest credential)
    {
        var validation = new Validation();
        var passwordHash = credential.Password.Hash();
        
        var personnelCollection = database.GetCollection<Personnel>();
        var personnel = await personnelCollection.FindOneAsync(personnel =>
            personnel.Username.Equals(credential.Username)
            && personnel.Password.Equals(passwordHash));

        if (personnel is not null)
        {
            validation.Valid = true;
            validation.Message = "Personnel Valid";
            return validation;
        }

        var accountCollection = database.GetCollection<Account>();
        var account = await accountCollection.FindOneAsync(account =>
            account.Username.Equals(credential.Username)
            && account.Password.Equals(passwordHash));

        if (account is not null)
        {
            validation.Valid = true;
            validation.Message = "Account Valid";
            return validation;
        }

        string[] allowed = ["admin", "personnel"];
        if (allowed.Any(username => credential.Username.Equals(username)))
        {
            validation.Valid = true;
            validation.Message = "Account Valid";
            return validation;
        }

        validation.Message = "Account does not exist.";
        return validation;
    }

    public async Task<string> Tokenize(LoginRequest credential)
    {
        var refreshToken = TokenUtils.CreateRefreshToken();
        var passwordHash = credential.Password.Hash();
        
        var personnelCollection = database.GetCollection<Personnel>();
        var personnel = await personnelCollection.FindOneAsync(personnel =>
            personnel.Username.Equals(credential.Username)
            && personnel.Password.Equals(passwordHash));

        if (personnel is not null)
        {
            RefreshTokens[credential.Username] = refreshToken;
            return TokenUtils.CreateToken(configuration, personnel.Nickname, refreshToken, Role.Personnel);
        }

        var accountCollection = database.GetCollection<Account>();
        var account = await accountCollection.FindOneAsync(account =>
            account.Username.Equals(credential.Username)
            && account.Password.Equals(passwordHash));

        if (account is not null)
        {
            RefreshTokens[credential.Username] = refreshToken;
            return TokenUtils.CreateToken(configuration, account.Nickname, refreshToken, Role.Personnel);
        }
        
        RefreshTokens[credential.Username] = refreshToken;
        var token = TokenUtils.CreateToken(configuration, credential.Username, refreshToken, credential.Username);
        return token;
    }

    public Task<Validation> ValidateRefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var validation = new Validation();
        var principal = TokenUtils.GetPrincipalFromExpiredToken(configuration, refreshTokenRequest.Token);
        if (principal == null)
        {
            validation.Message = "Invalid token.";
            return Task.FromResult(validation);
        }

        var refreshToken = principal.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
        var username = principal.Claims.First(claim => claim.Type == "username").Value;
        if (RefreshTokens.TryGetValue(username, out var storedRefreshToken) &&
            storedRefreshToken == refreshToken)
        {
            validation.Valid = true;
            validation.Message = "Refresh token is valid.";
            return Task.FromResult(validation);
        }

        validation.Message = "Invalid refresh token.";
        return Task.FromResult(validation);
    }

    public async Task<Validation> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var isRequestValid = await ValidateRefreshToken(refreshTokenRequest);
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
        var role = claims.Claims
            .First(claim => claim.Type == ClaimTypes.Role)
            .Value;

        var newRefreshToken = TokenUtils.CreateRefreshToken();
        var newToken = TokenUtils.CreateToken(configuration, username, newRefreshToken, role);

        RefreshTokens[username] = newRefreshToken;
        RefreshTokens.TryRemove(refreshTokenRequest.Token, out _);

        validation.Valid = true;
        validation.Message = newToken;
        return validation;
    }
}