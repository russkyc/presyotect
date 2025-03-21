using Presyotect.Core.Contracts;
using Presyotect.Core.Types;

namespace Presyotect.Features.Monitoring.Models;

public class MonitoredPrice : DbEntity
{
    public Guid ProductId { get; set; }
    public string? ProductSku { get; set; }
    public string ProductName { get; set; }
    public string ProductSize { get; set; }
    public decimal Price { get; set; }
    public string? Remarks { get; set; }
    public ICollection<string> ProductCategories { get; set; }
    public string ProductClassification { get; set; }
    public MonitoringStatus Status { get; set; }
    
    public Guid MonitoringScheduleId { get; set; }
    public string MonitoringIdentifier { get; set; }
    
    public Guid PersonnelId { get; set; }
    public string PersonnelName { get; set; }
    public Guid EstablishmentId { get; set; }
    public string EstablishmentName { get; set; }
    public string CityMunicipality { get; set; }
}