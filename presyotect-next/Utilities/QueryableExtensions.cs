using LiteDB.Queryable;
using Presyotect.Core.Models;

namespace Presyotect.Utilities;

public static class QueryableExtensions
{
    public static async Task<(Pagination Pagination, IEnumerable<T> Data)> PaginateAsync<T>(this IQueryable<T> items, int page, int pageSize,
        CancellationToken cancellationToken = new())
    {
        var totalItems = items.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var paginatedItems = items
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var paginatedData = await paginatedItems
            .ToListAsync(cancellationToken);

        var pagination = new Pagination
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Items = paginatedData.Count
        };
        
        return (pagination, paginatedData);
    }
    
    public static (Pagination Pagination, IEnumerable<T> Data) Paginate<T>(this IQueryable<T> items, int page, int pageSize,
        CancellationToken cancellationToken = new())
    {
        var totalItems = items.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var paginatedItems = items
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var paginatedData = paginatedItems
            .ToList();

        var pagination = new Pagination
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Items = paginatedData.Count
        };
        
        return (pagination, paginatedData);
    }
    
}