using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presyotect.Core.Contracts;
using Presyotect.Data;
using Presyotect.Utilities;
using DbEntity = Presyotect.Core.Contracts.DbEntity;

namespace Presyotect.Core.Abstractions;

public abstract class GenericEndpoint<T> where T : DbEntity
{
    protected static async Task<IResult> OnGetCount(
        HttpContext context,
        IDbContextFactory<AppDbContext> dbContextFactory,
        [FromQuery] string? query = null)
    {
        var response = new ResponseData<int>();
        await using var database = await dbContextFactory.CreateDbContextAsync();
        var count = await database
            .Set<T>()
            .CountAsync(p => p.Deleted == null);

        response.Success = true;
        response.Message = "Total count successful";
        response.Content = count;

        return Results.Ok(response);
    }

    protected static async Task<IResult> OnGetById(HttpContext context,
        [FromServices] IDbContextFactory<AppDbContext> dbContextFactory, [FromRoute] Guid id)
    {
        var response = new ResponseData<T>();
        await using var database = await dbContextFactory.CreateDbContextAsync();

        var product = await database
            .Set<T>()
            .FirstOrDefaultAsync(p => p.Deleted == null && p.Id == id);

        if (product is null)
        {
            response.Success = false;
            response.Message = "Record not found";
            return Results.Ok(response);
        }

        response.Success = true;
        response.Content = product;
        response.Message = "Record fetched successfully";

        return Results.Ok(response);
    }

    protected static async Task<IResult> OnGet(
        HttpContext context, 
        IDbContextFactory<AppDbContext> dbContextFactory,
        [FromQuery] string? query = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = int.MaxValue)
    {
        var response = new ResponseData<IEnumerable<T>>();
        await using var database = await dbContextFactory.CreateDbContextAsync();

        var queryable = database
            .Set<T>()
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
        IDbContextFactory<AppDbContext> dbContextFactory,
        T product)
    {
        var response = new ResponseData<T>();
        await using var database = await dbContextFactory.CreateDbContextAsync();
        var entry = await database.Set<T>().AddAsync(product);
        await database.SaveChangesAsync();

        response.Success = true;
        response.Content = entry.Entity;
        response.Message = "Record has been added.";

        return Results.Ok(response);
    }

    [Authorize]
    protected static async Task<IResult> OnUpdate(
        HttpContext context,
        IDbContextFactory<AppDbContext> dbContextFactory,
        T product)
    {
        var response = new ResponseData<T>();
        await using var database = await dbContextFactory.CreateDbContextAsync();
        var entry = database.Set<T>().Update(product);
        await database.SaveChangesAsync();

        response.Success = true;
        response.Content = entry.Entity;
        response.Message = "Record has been updated.";

        return Results.Ok(response);
    }

    [Authorize]
    protected static async Task<IResult> OnUpdateRange(
        HttpContext context,
        IDbContextFactory<AppDbContext> dbContextFactory,
        IEnumerable<T> product)
    {
        var response = new ResponseData<IEnumerable<T>>();
        await using var database = await dbContextFactory.CreateDbContextAsync();
        
        var responseContent = product as T[] ?? product.ToArray();
        database.Set<T>().UpdateRange(responseContent);
        await database.SaveChangesAsync();

        response.Success = true;
        response.Content = responseContent;
        response.Message = "Records have been updated.";

        return Results.Ok(response);
    }

    [Authorize]
    protected static async Task<IResult> OnDelete(
        HttpContext context,
        IDbContextFactory<AppDbContext> dbContextFactory,
        Guid id)
    {
        var response = new ResponseData<T>();
        await using var database = await dbContextFactory.CreateDbContextAsync();
        var entity = await database.Set<T>().FirstOrDefaultAsync(p => p.Deleted == null && p.Id == id);

        if (entity is null)
        {
            response.Message = "Record does not exist.";
            return Results.Ok(response);
        }

        database.Set<T>().Update(entity);
        await database.SaveChangesAsync();

        entity.Deleted = DateTime.Now;
        response.Success = true;
        response.Message = "Record has been deleted.";
        return Results.Ok(response);
    }
}