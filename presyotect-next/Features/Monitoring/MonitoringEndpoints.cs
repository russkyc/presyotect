using LiteDB;
using LiteDB.Async;
using LiteDB.Queryable;
using Microsoft.AspNetCore.Authorization;
using Presyotect.Core.Contracts;
using Presyotect.Features.Establishments.Models;
using Presyotect.Features.Monitoring.Models;
using Presyotect.Features.Products.Models;
using Presyotect.Features.Users.Models;
using Presyotect.Utilities;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        group.MapPost("/monitored-price", OnPostMonitoredPrice);
    }

    [Authorize]
    private static async Task<IResult> OnPostMonitoredPrice(
        HttpContext context,
        ILiteDatabaseAsync database,
        MonitoredPrice price)
    {
        var productId = new ObjectId(price.ProductId);
        var response = new ResponseData<MonitoredPrice>();
        var collection = database.GetCollection<Product>();

        var product = await collection.AsQueryable()
            .Include(product => product.MonitoredPrices)
            .FirstOrDefaultAsync(product => product.Id == productId);

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
            product.MonitoredPrices.Add(price);
        }

        Console.WriteLine(JsonSerializer.Serialize(product));
        var updated = await collection.UpdateAsync(product);
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
        string personnelId)
    {
        var response = new ResponseData<int>();
        var id = new ObjectId(personnelId);
        var personnel = await database.GetCollection<Personnel>()
            .AsQueryable()
            .FirstOrDefaultAsync(personnel => personnel.Deleted == null && personnel.Id == id);

        if (personnel is null)
        {
            response.Content = 0;
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        var monitoredEstablishmentIds = personnel.AssignedEstablishments
            .Select(establishmentId => new ObjectId(establishmentId));

        var monitoredEstablishments = await database.GetCollection<Establishment>()
            .AsQueryable()
            .CountAsync(establishment => establishment.Deleted == null && monitoredEstablishmentIds.Contains(establishment.Id));

        response.Success = true;
        response.Message = "Successfully found assigned establishments.";
        response.Content = monitoredEstablishments;
        
        return Results.Ok(response);
    }

    [Authorize]
    private static async Task<IResult> OnGetAssignedEstablishments(
        HttpContext context,
        ILiteDatabaseAsync database,
        string personnelId)
    {
        var response = new ResponseData<IEnumerable<MonitoredEstablishment>>();
        var id = new ObjectId(personnelId);
        var personnel = await database.GetCollection<Personnel>()
            .AsQueryable()
            .FirstOrDefaultAsync(personnel => personnel.Deleted == null && personnel.Id == id);

        if (personnel is null)
        {
            response.Content = [];
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        var monitoredEstablishmentIds = personnel.AssignedEstablishments
            .Select(establishmentId => new ObjectId(establishmentId));

        var monitoredEstablishments = await database.GetCollection<Establishment>()
            .AsQueryable()
            .Where(establishment => establishment.Deleted == null && monitoredEstablishmentIds.Contains(establishment.Id))
            .ToListAsync();

        var mappedEstablishments = monitoredEstablishments
            .Select(establishment => establishment.SimpleMap<MonitoredEstablishment>());

        response.Success = true;
        response.Message = "Successfully found assigned establishments.";
        response.Content = mappedEstablishments;
        
        return Results.Ok(response);
    }

    [Authorize]
    private static async Task<IResult> OnGetAllMonitoredProducts(
        HttpContext context,
        ILiteDatabaseAsync database,
        string personnelId)
    {
        var response = new ResponseData<IEnumerable<Product>>();
        var id = new ObjectId(personnelId);
        var personnel = await database.GetCollection<Personnel>()
            .AsQueryable()
            .FirstOrDefaultAsync(personnel => personnel.Deleted == null && personnel.Id == id);


        if (personnel is null)
        {
            response.Content = [];
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        var monitoredEstablishmentIds = personnel.AssignedEstablishments
            .Select(establishmentId => new ObjectId(establishmentId));

        var monitoredEstablishments = await database.GetCollection<Establishment>()
            .AsQueryable()
            .Where(establishment => establishment.Deleted == null && monitoredEstablishmentIds.Contains(establishment.Id))
            .ToListAsync();

        string[] classifications = [];
        monitoredEstablishments.ForEach(establishment =>
        {
            classifications = [..classifications, ..establishment.Classifications];
        });

        classifications = classifications.Distinct().ToArray();

        var products = await database.GetCollection<Product>()
            .AsQueryable()
            .Include(product => product.MonitoredPrices)
            .Where(product => product.Deleted == null && classifications.Contains(product.Classification))
            .ToListAsync();

        response.Content = products;
        response.Message = "Listing of assigned products successful.";
        
        return Results.Ok(response);
    }
}