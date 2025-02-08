using Presyotect.Core.Models;
using Presyotect.Features.Authentication.Models;
using Presyotect.Features.Authentication.Services;
using Presyotect.Utilities;

namespace Presyotect.Endpoints;

public class AuthEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("auth")
            .WithTags("Auth");
        group.MapPost("/login", OnLogin);
    }

    private static IResult OnLogin(HttpContext context, IConfiguration configuration, IAuthenticator authenticator,
        Account account)
    {
        var response = new ResponseData<string>();
        var validation = authenticator.Validate(account);
        
        if (!validation.Valid)
        {
            response.Success = false;
            response.Errors = [validation.Message ?? "Account cannot be found."];
            return Results.Ok(response);
        }

        var token = authenticator.Tokenize(account);
        response.Success = true;
        response.Message = "Login Successful";
        response.Content = token;
        
        return Results.Ok(response);
    }
}