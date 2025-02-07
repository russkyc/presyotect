using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Presyotect.Utilities;

namespace Presyotect.Endpoints;

public class ProductEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("products");
        group.MapGet("/", OnGet);
    }

    [Authorize]
    private static IResult OnGet(HttpContext context)
    {
        Console.Error.WriteLine(JsonSerializer.Serialize(context.Request.Headers));
        return Results.Ok(new
        {
            ProductName = "Sample Product"
        });
    }
}