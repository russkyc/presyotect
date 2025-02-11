using LiteDB;
using LiteDB.Async;
using LiteDB.Queryable;
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
    }

    private static async Task<IResult>  OnGetAssignedEstablishmentsCount(HttpContext context,
        ILiteDatabaseAsync database,
        string personnelId)
    {
        var response = new ResponseData<int>();
        var personnel = await database.GetCollection<Personnel>()
            .FindByIdAsync(new ObjectId(personnelId));

        if (personnel is null)
        {
            response.Content = 0;
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        var monitoredEstablishmentIds = personnel.AssignedEstablishments
            .Select(id => new ObjectId(id));

        var monitoredEstablishments = await database.GetCollection<Establishment>()
            .AsQueryable()
            .CountAsync(establishment => monitoredEstablishmentIds.Contains(establishment.Id));

        response.Success = true;
        response.Message = "Successfully found assigned establishments.";
        response.Content = monitoredEstablishments;
        return Results.Ok(response);
    }

    private static async Task<IResult> OnGetAssignedEstablishments(
        HttpContext context,
        ILiteDatabaseAsync database,
        string personnelId)
    {
        var response = new ResponseData<IEnumerable<MonitoredEstablishment>>();
        var personnel = await database.GetCollection<Personnel>()
            .FindByIdAsync(new ObjectId(personnelId));

        if (personnel is null)
        {
            response.Content = [];
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        var monitoredEstablishmentIds = personnel.AssignedEstablishments
            .Select(id => new ObjectId(id));

        var monitoredEstablishments = await database.GetCollection<Establishment>()
            .AsQueryable()
            .Where(establishment => monitoredEstablishmentIds.Contains(establishment.Id))
            .ToListAsync();

        var mappedEstablishments = monitoredEstablishments
            .Select(establishment => establishment.SimpleMap<MonitoredEstablishment>());

        response.Success = true;
        response.Message = "Successfully found assigned establishments.";
        response.Content = mappedEstablishments;
        return Results.Ok(response);
    }
    
    private static async Task<IResult> OnGetAllMonitoredProducts(
        HttpContext context,
        ILiteDatabaseAsync database,
        string personnelId)
    {
        var response = new ResponseData<IEnumerable<Product>>();
        var personnel = await database.GetCollection<Personnel>()
            .FindByIdAsync(new ObjectId(personnelId));

        if (personnel is null)
        {
            response.Content = [];
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        var monitoredEstablishmentIds = personnel.AssignedEstablishments
            .Select(id => new ObjectId(id));

        var monitoredEstablishments = await database.GetCollection<Establishment>()
            .AsQueryable()
            .Where(establishment => monitoredEstablishmentIds.Contains(establishment.Id))
            .ToListAsync();

        string[] classifications = [];
        monitoredEstablishments.ForEach(establishment =>
        {
            classifications = [..classifications,..establishment.Classifications];
        });

        classifications = classifications.Distinct().ToArray();

        var products = await database.GetCollection<Product>()
            .AsQueryable()
            .Where(product => classifications.Contains(product.Classification))
            .ToListAsync();
        
        response.Content = products;
        response.Message = "Listing of assigned products successful.";
        return Results.Ok(response);
    }
}