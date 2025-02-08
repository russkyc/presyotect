using Presyotect.Core.Entities;
using Presyotect.Utilities;

namespace Presyotect.Endpoints;

public class EstablishmentEndpoints : GenericEndpoint<Establishment>, IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("establishments")
            .WithTags("Establishments");

        group.MapGet("/", OnGet);
        group.MapGet("/count", OnGetCount);
        group.MapPost("/", OnAdd);
        group.MapPatch("/", OnUpdate);
        group.MapPatch("/range", OnUpdateRange);
        group.MapDelete("/{id}", OnDelete);
    }
}