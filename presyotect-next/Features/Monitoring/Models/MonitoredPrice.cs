using Presyotect.Core.Contracts;

namespace Presyotect.Features.Monitoring.Models;

public class MonitoredPrice : DbEntity
{
    public string ProductId { get; set; }
    public string PersonnelId { get; set; }
    public string EstablishmentId { get; set; }
    public float Price { get; set; }
    public string? Remarks { get; set; }
    public string Status { get; set; }
}