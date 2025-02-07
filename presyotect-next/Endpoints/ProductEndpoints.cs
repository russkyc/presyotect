using Presyotect.Utilities;

namespace Presyotect.Endpoints;

public class ProductEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("products");
        group.MapGet("/", OnGet)
            .RequireAuthorization();
    }

    private static IResult OnGet(HttpContext context)
    {
        return Results.Ok(new
        {
            ProductName = "Sample Product"
        });
    }
}