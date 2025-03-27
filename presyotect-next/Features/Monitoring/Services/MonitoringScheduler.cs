using Hangfire;
using Microsoft.EntityFrameworkCore;
using Presyotect.Data;
using Presyotect.Features.Monitoring.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Monitoring.Services;

public class MonitoringScheduler
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public MonitoringScheduler(IRecurringJobManager recurringJobManager, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        recurringJobManager.AddOrUpdate(
            "check-monitoring-schedules",
            () => CreateSchedule(),
            Hangfire.Cron.Minutely);
    }

    public async Task CreateSchedule()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var startOfWeek = DateTime.Now.StartOfWeek();
        var endOfWeek = DateTime.Now.EndOfWeek();
        var monitoringId = startOfWeek.AsIdentifier();
        var entryCount = await dbContext.MonitoringSchedules
            .CountAsync(schedule => schedule.MonitoringId.Equals(monitoringId));

        if (entryCount > 0)
        {
            Console.WriteLine("Monitoring schedule already exists.");
            return;
        }
        
        var schedule = new MonitoringSchedule
        {
            MonitoringId = monitoringId,
            StartDate = startOfWeek,
            EndDate = endOfWeek
        };

        await dbContext.MonitoringSchedules.AddAsync(schedule);
        await dbContext.SaveChangesAsync();
    }
}