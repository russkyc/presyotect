using Presyotect.Core.Abstractions;
using Presyotect.Features.Establishments.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Establishments;

public class EstablishmentEndpoints : GenericEndpoint<Establishment>, IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("establishments")
            .WithTags("Establishments");

        group.MapGet("/", OnGet);
        group.MapGet("/{id}",OnGetById);
        group.MapGet("/count", OnGetCount);
        group.MapPost("/", OnAdd);
        group.MapPatch("/", OnUpdate);
        group.MapPatch("/range", OnUpdateRange);
        group.MapDelete("/{id}", OnDelete);
    }
}