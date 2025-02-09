using Presyotect.Core.Abstractions;
using Presyotect.Features.Products.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Products;

public class ProductEndpoints : GenericEndpoint<Product>, IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("products")
            .WithTags("Products");

        group.MapGet("/", OnGet);
        group.MapGet("/count", OnGetCount);
        group.MapPost("/", OnAdd);
        group.MapPatch("/", OnUpdate);
        group.MapPatch("/range", OnUpdateRange);
        group.MapDelete("/{id}", OnDelete);
    }

}