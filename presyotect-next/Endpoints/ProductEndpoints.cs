using System.Text.Json;
using LiteDB.Async;
using Microsoft.AspNetCore.Authorization;
using Presyotect.Core.Entities;
using Presyotect.Core.Models;
using Presyotect.Utilities;

namespace Presyotect.Endpoints;

public class ProductEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("products");
        
        group.MapGet("/", OnGet);
        group.MapPost("/", OnAddProduct);
    }

    [Authorize(Roles = "admin")]
    private static async Task<IResult> OnAddProduct(HttpContext context, ILiteDatabaseAsync database, Product product)
    {
        var response = new ResponseData<Product>();
        var collection = database.GetCollection<Product>();
        var id = await collection.InsertAsync(product);
        
        product.Id = id.AsObjectId;
        response.Success = true;
        response.Content = product;
        response.Message = $"{product.Name} has been added to products.";
        
        return Results.Ok(response);
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