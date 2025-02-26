using LiteDB;
using Presyotect.Core.Contracts;

namespace Presyotect.Features.Monitoring.Models;

public class MonitoredSrp : DbEntity
{
    public ObjectId ProductId { get; set; }
    public ObjectId PersonnelId { get; set; }
    public ObjectId EstablishmentId { get; set; }
    public float Value { get; set; }
    public string? Remarks { get; set; }
    public string Status { get; set; }
}