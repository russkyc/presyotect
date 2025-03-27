using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Presyotect.Core.Contracts;
using Presyotect.Data;
using Presyotect.Features.Monitoring.Models;
using Presyotect.Features.Products.Models;
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
        IDbContextFactory<AppDbContext> dbContextFactory,
        string? cityMunicipality = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var response = new ResponseData<IEnumerable<MonitoredPrice>>();
        await using var database = await dbContextFactory.CreateDbContextAsync();

        var queryable = database.MonitoredPrices
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(cityMunicipality))
        {
            queryable = queryable.Where(m => m.CityMunicipality.Equals(cityMunicipality));
        }

        if (startDate is not null)
        {
            queryable = queryable.Where(m => m.Created > startDate);
        }
        else if (startDate is not null && endDate is not null)
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
        IDbContextFactory<AppDbContext> dbContextFactory,
        MonitoredPrice price)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var currentMonitoringId = DateTime.Now.StartOfWeek().AsIdentifier();
        var response = new ResponseData<MonitoredPrice>();

        var schedule = await dbContext
            .MonitoringSchedules
            .FirstOrDefaultAsync(p => p.Deleted == null && p.MonitoringId.Equals(currentMonitoringId));

        if (schedule is null)
        {
            response.Errors = ["No schedule for the current week."];
            return Results.Ok(response);
        }

        price.MonitoringIdentifier = schedule.MonitoringId;
        price.MonitoringScheduleId = schedule.Id;

        var entry = await dbContext.MonitoredPrices.AddAsync(price);
        await dbContext.SaveChangesAsync();

        response.Success = true;
        response.Message = "Price entry recorded.";
        response.Content = entry.Entity;

        return Results.Ok(response);
    }

    [Authorize]
    private static async Task<IResult> OnGetAssignedEstablishmentsCount(HttpContext context,
        IDbContextFactory<AppDbContext> dbContextFactory,
        Guid personnelId)
    {
        var response = new ResponseData<int>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var personnel = await dbContext.Personnel
            .Include(p => p.AssignedEstablishments)
            .FirstOrDefaultAsync(p => p.Deleted == null && p.Id == personnelId);

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
        IDbContextFactory<AppDbContext> dbContextFactory,
        Guid personnelId)
    {
        var response = new ResponseData<IEnumerable<MonitoredEstablishment>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var personnel = await dbContext.Personnel
            .Include(p => p.AssignedEstablishments)
            .FirstOrDefaultAsync(p => p.Deleted == null && p.Id == personnelId);

        if (personnel is null)
        {
            response.Content = [];
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        var establishments = await dbContext.Establishments
            .Where(e => personnel.AssignedEstablishments.Contains(e.Id))
            .ToListAsync();

        var mappedEstablishments = establishments.Select(e => e.SimpleMap<MonitoredEstablishment>());

        response.Success = true;
        response.Message = "Successfully found assigned establishments.";
        response.Content = mappedEstablishments;

        return Results.Ok(response);
    }

    [Authorize]
    private static async Task<IResult> OnGetAllMonitoredProducts(
        HttpContext context,
        IDbContextFactory<AppDbContext> dbContextFactory,
        Guid personnelId)
    {
        var response = new ResponseData<IEnumerable<Product>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        
        var personnel = await dbContext.Personnel
            .Include(p => p.AssignedEstablishments)
            .FirstOrDefaultAsync(p => p.Deleted == null && p.Id == personnelId);


        if (personnel is null)
        {
            response.Content = [];
            response.Errors = ["Invalid personnel id."];
            return Results.Ok(response);
        }

        string[] classifications = [];

        var establishments = await dbContext.Establishments
            .Where(e => personnel.AssignedEstablishments.Contains(e.Id))
            .ToListAsync();

        establishments.ForEach(e => classifications = [..e.Classifications]);

        classifications = classifications.Distinct().ToArray();

        var products = await dbContext
            .Products
            .Where(p => p.Deleted == null && classifications.Contains(p.Classification))
            .ToListAsync();

        response.Content = products;
        response.Message = "Listing of assigned products successful.";

        return Results.Ok(response);
    }
}