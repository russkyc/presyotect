using System.Text.Json;
using LiteDB.Async;
using Microsoft.AspNetCore.Authorization;
using Presyotect.Core.Abstractions;
using Presyotect.Core.Contracts;
using Presyotect.Features.Users.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Users;

public class PersonnelEndpoints : GenericEndpoint<Personnel>, IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("personnel")
            .WithTags("Personnel");

        group.MapGet("/", OnGet);
        group.MapGet("/count", OnGetCount);
        group.MapPost("/", OnAddPersonnel);
        group.MapPatch("/", OnUpdate);
        group.MapPatch("/range", OnUpdateRange);
        group.MapDelete("/{id}", OnDelete);
    }
    
    [Authorize(Roles = "admin, superadmin")]
    protected static async Task<IResult> OnAddPersonnel(
        HttpContext context,
        ILiteDatabaseAsync database,
        Personnel personnel)
    {
        personnel.Password = personnel.Password.Hash();
        Console.WriteLine(JsonSerializer.Serialize(personnel));
        var response = new ResponseData<Personnel>();
        var collection = database.GetCollection<Personnel>();
        var id = await collection.InsertAsync(personnel);

        personnel.Id = id;
        response.Success = true;
        response.Content = personnel;
        response.Message = "Personnel has been added.";

        return Results.Ok(response);
    }
}