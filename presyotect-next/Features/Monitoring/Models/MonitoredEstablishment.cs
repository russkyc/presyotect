using Presyotect.Core.Contracts;

namespace Presyotect.Features.Monitoring.Models;

public class MonitoredEstablishment : DbEntity
{
    
    // General
    public string Name { get; set; }
    public string Status { get; set; }
    public string CityMunicipality { get; set; }
    public string CompleteAddress { get; set; }
    
    // Classification
    public ICollection<string> Categories { get; set; }
    public ICollection<string> Classifications { get; set; }
    public ICollection<string> SubClassifications { get; set; }
    public ICollection<string> Tags { get; set; }
    
    // Ownership
    public ICollection<string> Owners { get; set; }
    
    // Optional Contact
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? Twitter { get; set; }
}