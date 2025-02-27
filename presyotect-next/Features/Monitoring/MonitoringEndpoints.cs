using System.Collections.Immutable;
using LiteDB.Async;
using LiteDB.Queryable;
using Microsoft.AspNetCore.Authorization;
using Presyotect.Core.Contracts;
using Presyotect.Features.Establishments.Models;
using Presyotect.Features.Monitoring.Models;
using Presyotect.Features.Products.Models;
using Presyotect.Features.Users.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Monitoring;

public class MonitoringEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapApiGroup("monitoring")
            .WithTags("Price Monitoring");

        group.MapGet("/assigned-establishments/count", OnGetAssignedEstablishmentsCount);
        group.MapGet("/assigned-establishments", OnGetAssignedEstablishments);
        group.MapGet("/all-monitored-products", OnGetAllMonitoredProducts);
        group.MapGet("/monitored-prices", OnGetMonitoredPrices);
        group.MapPost("/monitored-price", OnPostMonitoredPrice);
    }

    private static async Task<IResult> OnGetMonitoredPrices(
        HttpContext context,
        ILiteDatabaseAsync database,
        string? cityMunicipality = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var response = new ResponseData<IEnumerable<MonitoredPrice>>();
        var collection = database.GetCollection<MonitoredPrice>();

        var queryable = collection.AsQueryable();

        if (!string.IsNullOrWhiteSpace(cityMunicipality))
        {
            queryable = queryable.Where(m => m.CityMunicipality.Equals(cityMunicipality));
        }
        
        if (startDate is not null)
        {
            queryable = queryable.Where(m => m.Created > startDate);
        } else if (startDate is not null && endDate is not null)
        {
            queryable = queryable.Where(m => m.Created >= startDate
                                             && m.Created <= endDate);
        }

        var monitoredPrices = await queryable.ToListAsync();

        response.Success = true;
        response.Message = "Successfully listed monitoring information";
        response.Content = monitoredPrices;
        
        return Results.Empty;
    }

    [Authorize]
    private static async Task<IResult> OnPostMonitoredPrice(
        HttpContext context,
        ILiteDatabaseAsync database,
        MonitoredPrice price)
    {
        var response = new ResponseData<MonitoredPrice>();
        var collection = database.GetCollection<Product>();

        var product = await collection
            .FindOneAsync(p => p.Deleted == null && p.Id == price.ProductId);

        if (product is null)
        {
            response.Errors = ["Product does not exist."];
            return Results.Ok(response);
        }

        if (product.MonitoredPrices is null)
        {
            product.MonitoredPrices = [price];
        }
        else
        {
            product.MonitoredPrices = [price, ..product.MonitoredPrices];
        }

        var updated = await collection.UpdateAsync(product);
        await database.CommitAsync();
        if (!updated)
        {
            response.Errors = ["Unable to record price movement."];
            return Results.Ok(response);
        }

        response.Success = updated;
        response.Message = "Price entry recorded.";
        response.Content = price;

        return Results.Ok(response);
    }

    [Authorize]
    private static async Task<IResult> OnGetAssignedEstablishmentsCount(HttpContext context,
        ILiteDatabaseAsync database,
        Guid personnelId)
    {
        var response = new ResponseData<int>();
        var personnel = await database.GetCollection<Personnel>()
            .Include(p => p.AssignedEstablishments)
            .FindOneAsync(p => p.Deleted == null && p.Id == personnelId);

        if (personnel is null)
        {
            response.Content = 0;
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        response.Success = true;
        response.Message = "Successfully found assigned establishments.";
        response.Content = personnel.AssignedEstablishments.Count;

        return Results.Ok(response);
    }

    [Authorize]
    private static async Task<IResult> OnGetAssignedEstablishments(
        HttpContext context,
        ILiteDatabaseAsync database,
        Guid personnelId)
    {
        var response = new ResponseData<IEnumerable<MonitoredEstablishment>>();
        var personnel = await database.GetCollection<Personnel>()
            .Include(p => p.AssignedEstablishments)
            .FindOneAsync(p => p.Deleted == null && p.Id == personnelId);

        if (personnel is null)
        {
            response.Content = [];
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        var establishments = await database.GetCollection<Establishment>()
            .FindAsync(e => personnel.AssignedEstablishments.Contains(e.Id));

        var mappedEstablishments = establishments.Select(e => e.SimpleMap<MonitoredEstablishment>());

        response.Success = true;
        response.Message = "Successfully found assigned establishments.";
        response.Content = mappedEstablishments;

        return Results.Ok(response);
    }

    [Authorize]
    private static async Task<IResult> OnGetAllMonitoredProducts(
        HttpContext context,
        ILiteDatabaseAsync database,
        Guid personnelId)
    {
        var response = new ResponseData<IEnumerable<Product>>();
        var personnel = await database.GetCollection<Personnel>()
            .Include(p => p.AssignedEstablishments)
            .FindOneAsync(p => p.Deleted == null && p.Id == personnelId);


        if (personnel is null)
        {
            response.Content = [];
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        string[] classifications = [];

        var establishments = await database.GetCollection<Establishment>()
            .FindAsync(e => personnel.AssignedEstablishments.Contains(e.Id));
        
        establishments.ToImmutableList()
            .ForEach(e => classifications = [..e.Classifications]);

        classifications = classifications.Distinct().ToArray();

        var products = await database
            .GetCollection<Product>()
            .Include(p => p.MonitoredPrices)
            .FindAsync(p => p.Deleted == null && classifications.Contains(p.Classification));

        response.Content = products;
        response.Message = "Listing of assigned products successful.";

        return Results.Ok(response);
    }
}