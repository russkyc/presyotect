namespace Presyotect.Features.Monitoring.Models;

public class MonitoredEstablishment
{
    public string Name { get; set; }
    public string CityMunicipality { get; set; }
    public string CompleteAddress { get; set; }
    public string[] Categories { get; set; }
    public string[] Classifications { get; set; }
    public string[] SubClassifications { get; set; }
}