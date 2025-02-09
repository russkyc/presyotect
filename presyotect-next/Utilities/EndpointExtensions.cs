using Presyotect.Core.Constants;

namespace Presyotect.Utilities;

public static class EndpointExtensions
{
    public static RouteGroupBuilder MapApiGroup(this IEndpointRouteBuilder endpointRouteBuilder, string prefix)
    {
        return endpointRouteBuilder.MapGroup($"{Strings.ApiPrefix}/{prefix}");
    }
}