using System.Linq.Dynamic.Core;
using LiteDB;
using LiteDB.Async;
using LiteDB.Queryable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presyotect.Core.Models;
using Presyotect.Utilities;

namespace Presyotect.Endpoints;

public abstract class GenericEndpoint<T> where T : DbEntity
{
    protected static async Task<IResult> OnGetCount(
        HttpContext context,
        ILiteDatabaseAsync database,
        [FromQuery] string? query = null)
    {
        var response = new ResponseData<int>();
        var collection = database.GetCollection<T>();
        var count = await collection.AsQueryable()
            .Where(p => p.Deleted == null)
            .CountAsync();

        response.Success = true;
        response.Message = "Total count successful";
        response.Content = count;

        return Results.Ok(response);
    }

    protected static async Task<IResult> OnGet(
        HttpContext context,
        ILiteDatabaseAsync database,
        [FromQuery] string? query = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = int.MaxValue)
    {
        var response = new ResponseData<IEnumerable<T>>();
        var collection = database.GetCollection<T>();

        var queryable = collection
            .AsQueryable()
            .Where(p => p.Deleted == null);

        if (query is not null)
        {
            queryable = queryable.Where(query);
        }

        if (orderBy is not null)
        {
            queryable = queryable.OrderBy(orderBy);
        }

        var paginatedData = await queryable.PaginateAsync(page, pageSize);

        response.Success = true;
        response.Content = paginatedData.Data;
        response.Pagination = paginatedData.Pagination;
        response.Message = "Records fetched successfully";

        return Results.Ok(response);
    }

    [Authorize]
    protected static async Task<IResult> OnAdd(
        HttpContext context,
        ILiteDatabaseAsync database,
        T product)
    {
        var response = new ResponseData<T>();
        var collection = database.GetCollection<T>();
        var id = await collection.InsertAsync(product);

        product.Id = id;
        response.Success = true;
        response.Content = product;
        response.Message = "Record has been added.";

        return Results.Ok(response);
    }

    [Authorize]
    protected static async Task<IResult> OnUpdate(
        HttpContext context,
        ILiteDatabaseAsync database,
        T product)
    {
        var response = new ResponseData<T>();
        var collection = database.GetCollection<T>();
        var updated = await collection.UpdateAsync(product);

        response.Success = updated;
        response.Content = product;
        response.Message = "Record has been updated.";

        return Results.Ok(response);
    }
    
    [Authorize]
    protected static async Task<IResult> OnUpdateRange(
        HttpContext context,
        ILiteDatabaseAsync database,
        IEnumerable<T> product)
    {
        var response = new ResponseData<IEnumerable<T>>();
        var collection = database.GetCollection<T>();
        var responseContent = product as T[] ?? product.ToArray();
        var updated = await collection.UpdateAsync(responseContent);

        response.Success = updated > 0;
        response.Content = responseContent;
        response.Message = "Records have been updated.";

        return Results.Ok(response);
    }

    [Authorize]
    protected static async Task<IResult> OnDelete(
        HttpContext context,
        ILiteDatabaseAsync database,
        string id)
    {
        var response = new ResponseData<T>();
        var collection = database.GetCollection<T>();
        var product = await collection
            .AsQueryable()
            .FirstOrDefaultAsync(p => p.Deleted == null && p.Id == new ObjectId(id));

        if (product is null)
        {
            response.Message = "Record does not exist.";
            return Results.Ok(response);
        }

        product.Deleted = DateTime.Now;
        response.Success = await collection.UpdateAsync(product);
        response.Message = "Record has been deleted.";
        return Results.Ok(response);
    }
}