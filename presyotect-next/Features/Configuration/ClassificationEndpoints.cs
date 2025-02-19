using Presyotect.Core.Abstractions;
using Presyotect.Features.Configuration.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Configuration;

public class ClassificationEndpoints : GenericEndpoint<Classification>, IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("classifications")
            .WithTags("Classifications");

        group.MapGet("/", OnGet);
        group.MapGet("/count", OnGetCount);
        group.MapPost("/", OnAdd);
        group.MapPatch("/", OnUpdate);
        group.MapPatch("/range", OnUpdateRange);
        group.MapDelete("/{id}", OnDelete);

    }
}