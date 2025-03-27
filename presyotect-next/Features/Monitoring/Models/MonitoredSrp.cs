using Presyotect.Core.Contracts;

namespace Presyotect.Features.Monitoring.Models;

public class MonitoredSrp : DbEntity
{
    public Guid ProductId { get; set; }
    public Guid PersonnelId { get; set; }
    public Guid EstablishmentId { get; set; }
    public float Value { get; set; }
    public string? Remarks { get; set; }
    public string Status { get; set; }
}