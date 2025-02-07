﻿using Presyotect.Core.Entities;
using Presyotect.Utilities;

namespace Presyotect.Endpoints;

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