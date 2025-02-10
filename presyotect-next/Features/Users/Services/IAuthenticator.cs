using Presyotect.Features.Users.Models;

namespace Presyotect.Features.Users.Services;

public interface IAuthenticator
{
    public Task<Validation> Validate(LoginRequest credential);
    public Task<string> Tokenize(LoginRequest credential);
    public Task<Validation> ValidateRefreshToken(RefreshTokenRequest refreshTokenRequest);
    public Task<Validation> RefreshToken(RefreshTokenRequest refreshTokenRequest);
}