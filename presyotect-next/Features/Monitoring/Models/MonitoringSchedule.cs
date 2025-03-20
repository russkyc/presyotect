using Presyotect.Core.Contracts;
using Presyotect.Core.Types;

namespace Presyotect.Features.Monitoring.Models;

public class MonitoringSchedule : DbEntity
{
    public DateTime MonitoringStartDate { get; set; }
    public DateTime MonitoringEndDate { get; set; }
    public MonitoringInterval Interval { get; set; }
}