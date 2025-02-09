using Presyotect.Core.Contracts;
using Presyotect.Features.Users.Models;
using Presyotect.Features.Users.Services;
using Presyotect.Utilities;

namespace Presyotect.Features.Users;

public class AuthEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("auth")
            .WithTags("Auth");
        group.MapPost("/login", OnLogin);
        group.MapPost("/refresh-token", OnRefreshToken);
    }

    private static IResult OnLogin(HttpContext context, IConfiguration configuration, IAuthenticator authenticator,
        User user)
    {
        var response = new ResponseData<string>();
        var validation = authenticator.Validate(user);

        if (!validation.Valid)
        {
            response.Success = false;
            response.Errors = [validation.Message ?? "User cannot be found."];
            return Results.Ok(response);
        }

        user.Roles = [user.Username]; // Ensure roles are properly set
        var token = authenticator.Tokenize(user);
        response.Success = true;
        response.Message = "Login Successful";
        response.Content = token;

        return Results.Ok(response);
    }

    private static IResult OnRefreshToken(HttpContext context, IConfiguration configuration,
        IAuthenticator authenticator,
        RefreshTokenRequest refreshTokenRequest)
    {
        var response = new ResponseData<string>();
        var validation = authenticator.ValidateRefreshToken(refreshTokenRequest);

        if (!validation.Valid)
        {
            response.Success = false;
            response.Errors = ["Invalid refresh token."];
            return Results.Ok(response);
        }

        var newTokenValidation = authenticator.RefreshToken(refreshTokenRequest);

        if (!newTokenValidation.Valid)
        {
            response.Success = false;
            response.Errors = ["Invalid refresh token."];
            return Results.Ok(response);
        }

        response.Success = true;
        response.Message = "Refresh token validated, Issued new token.";
        response.Content = newTokenValidation.Message;

        return Results.Ok(response);
    }
}