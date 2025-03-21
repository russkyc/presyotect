using Hangfire;
using LiteDB.Async;
using Presyotect.Features.Monitoring.Models;
using Presyotect.Utilities;

namespace Presyotect.Features.Monitoring.Services;

public class MonitoringScheduler
{
    private readonly ILiteDatabaseAsync _database;
    private readonly IRecurringJobManager _recurringJobManager;

    public MonitoringScheduler(IRecurringJobManager recurringJobManager, ILiteDatabaseAsync database)
    {
        _recurringJobManager = recurringJobManager;
        _database = database;
        
        recurringJobManager.AddOrUpdate(
            "check-monitoring-schedules",
            () => CreateSchedule(),
            Hangfire.Cron.Minutely);
    }

    public async Task CreateSchedule()
    {
        var collection = _database.GetCollection<MonitoringSchedule>();
        var startOfWeek = DateTime.Now.StartOfWeek();
        var endOfWeek = DateTime.Now.EndOfWeek();
        var monitoringId = startOfWeek.AsIdentifier();
        var entryCount = await collection.CountAsync(schedule => schedule.MonitoringId.Equals(monitoringId));

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

        await collection.InsertAsync(schedule);
    }
}