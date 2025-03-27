using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Presyotect.Core.Abstractions;
using Presyotect.Core.Contracts;
using Presyotect.Data;
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
        IDbContextFactory<AppDbContext> dbContextFactory,
        Personnel personnel)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        personnel.Password = personnel.Password.Hash();
        Console.WriteLine(JsonSerializer.Serialize(personnel));
        var response = new ResponseData<Personnel>();
        var entry = await dbContext.Personnel.AddAsync(personnel);
        await dbContext.SaveChangesAsync();

        response.Success = true;
        response.Content = entry.Entity;
        response.Message = "Personnel has been added.";

        return Results.Ok(response);
    }
}