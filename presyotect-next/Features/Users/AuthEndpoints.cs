using System.Text.Json;
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

    private static async Task<IResult> OnLogin(HttpContext context, IConfiguration configuration, IAuthenticator authenticator,
        LoginRequest loginRequest)
    {
        var response = new ResponseData<string>();
        var validation = await authenticator.Validate(loginRequest);

        if (!validation.Valid)
        {
            response.Success = false;
            response.Errors = [validation.Message ?? "Personnel cannot be found."];
            return Results.Ok(response);
        }

        var token = await authenticator.Tokenize(loginRequest);
        response.Success = true;
        response.Message = "Login Successful";
        response.Content = token;

        return Results.Ok(response);
    }

    private static async Task<IResult> OnRefreshToken(HttpContext context, IConfiguration configuration,
        IAuthenticator authenticator,
        RefreshTokenRequest refreshTokenRequest)
    {
        var response = new ResponseData<string>();
        var validation = await authenticator.ValidateRefreshToken(refreshTokenRequest);

        if (!validation.Valid)
        {
            response.Success = false;
            response.Errors = ["Invalid refresh token."];
            return Results.Ok(response);
        }

        var newTokenValidation = await authenticator.RefreshToken(refreshTokenRequest);

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