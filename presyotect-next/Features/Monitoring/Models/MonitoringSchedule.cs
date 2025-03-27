using Presyotect.Core.Contracts;

namespace Presyotect.Features.Monitoring.Models;

public class MonitoringSchedule : DbEntity
{
    public string MonitoringId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}