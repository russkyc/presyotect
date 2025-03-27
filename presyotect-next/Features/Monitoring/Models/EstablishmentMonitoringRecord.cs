namespace Presyotect.Features.Monitoring.Models;

public class EstablishmentMonitoringRecord
{
    public Guid EstablishmentId { get; set; }
    public Guid MonitoringPersonnel { get; set; }
    public string Name { get; set; }
    public string CityMunicipality { get; set; }
    public DateTime MonitoringDate { get; set; }
}