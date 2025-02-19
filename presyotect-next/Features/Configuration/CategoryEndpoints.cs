using System.Linq.Dynamic.Core;
using LiteDB.Async;
using LiteDB.Queryable;
using Microsoft.AspNetCore.Mvc;
using Presyotect.Core.Abstractions;
using Presyotect.Core.Contracts;
using Presyotect.Features.Configuration.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Configuration;

public class CategoryEndpoints : GenericEndpoint<Category>, IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("categories")
            .WithTags("Categories");

        group.MapGet("/", OnGetCategories);
        group.MapGet("/count", OnGetCategoriesCount);
        group.MapPost("/", OnAdd);
        group.MapPatch("/", OnUpdate);
        group.MapPatch("/range", OnUpdateRange);
        group.MapDelete("/{id}", OnDelete);
    }

    protected static async Task<IResult> OnGetCategoriesCount(
        HttpContext context,
        ILiteDatabaseAsync database,
        [FromQuery] string? group = null,
        [FromQuery] string? query = null)
    {
        var response = new ResponseData<int>();
        var collection = database.GetCollection<Category>();
        var queryable = collection
            .AsQueryable()
            .Where(p => p.Deleted == null);

        if (!string.IsNullOrWhiteSpace(group))
        {
            queryable = queryable.Where(c => c.Group.Equals(group));
        }

        var count = await queryable.CountAsync();

        response.Success = true;
        response.Message = "Total count successful";
        response.Content = count;

        return Results.Ok(response);
    }

    protected static async Task<IResult> OnGetCategories(
        HttpContext context,
        ILiteDatabaseAsync database,
        [FromQuery] string? group = null,
        [FromQuery] string? query = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = int.MaxValue)
    {
        var response = new ResponseData<IEnumerable<Category>>();
        var collection = database.GetCollection<Category>();

        var queryable = collection
            .AsQueryable()
            .Where(p => p.Deleted == null);

        if (query is not null)
        {
            queryable = queryable.Where(query);
        }

        if (!string.IsNullOrWhiteSpace(group))
        {
            queryable = queryable.Where(c => c.Group.Equals(group));
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
}