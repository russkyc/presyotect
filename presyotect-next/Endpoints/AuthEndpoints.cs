using Presyotect.Utilities;

namespace Presyotect.Endpoints;

public class AuthEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("auth");
        group.MapPost("/login", OnLogin);
    }

    private static IResult OnLogin(HttpContext context, IConfiguration configuration, object data)
    {
        var token = TokenUtils.CreateToken(configuration, "admin", "Admin");
        return Results.Ok(new
        {
            Data = token
        });
    }
}