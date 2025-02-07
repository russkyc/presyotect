using LiteDB.Async;
using LiteDB.Queryable;
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

        group.MapPost("/", OnAddProduct);
        group.MapGet("/", OnGet);
        group.MapGet("/count", OnGetCount);
        group.MapDelete("/{id:int}", OnDelete);
    }

    private static async Task<IResult> OnDelete(HttpContext context, ILiteDatabaseAsync database, int id)
    {
        var response = new ResponseData<Product>();
        var collection = database.GetCollection<Product>();
        var product = await collection
            .AsQueryable()
            .FirstOrDefaultAsync(product => product.Deleted == null && product.Id == id);

        if (product is null)
        {
            response.Message = "Product does not exist.";
            return Results.Ok(response);
        }
        
        product.Deleted = DateTime.Now;
        response.Success = await collection.UpdateAsync(product);
        response.Message = $"{product.Name} has been deleted.";
        return Results.Ok(response);
    }

    private static async Task<IResult> OnGetCount(HttpContext context, ILiteDatabaseAsync database)
    {
        var response = new ResponseData<int>();
        var collection = database.GetCollection<Product>();
        var count = await collection.AsQueryable()
            .Where(product => product.Deleted == null)
            .CountAsync();

        response.Success = true;
        response.Message = "Total count successful";
        response.Content = count;

        return Results.Ok(response);
    }

    [Authorize(Roles = "admin")]
    private static async Task<IResult> OnAddProduct(HttpContext context, ILiteDatabaseAsync database, Product product)
    {
        var response = new ResponseData<Product>();
        var collection = database.GetCollection<Product>();
        var id = await collection.InsertAsync(product);

        product.Id = id;
        response.Success = true;
        response.Content = product;
        response.Message = $"{product.Name} has been added to products.";

        return Results.Ok(response);
    }

    [Authorize]
    private static async Task<IResult> OnGet(HttpContext context, ILiteDatabaseAsync database)
    {
        var response = new ResponseData<IEnumerable<Product>>();
        var collection = database.GetCollection<Product>();

        var products = await collection.AsQueryable()
            .Where(product => product.Deleted == null)
            .ToListAsync();
        response.Success = true;
        response.Content = products;

        return Results.Ok(response);
    }
}