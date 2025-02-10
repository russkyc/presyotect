using Presyotect.Core.Contracts;

namespace Presyotect.Features.Establishments.Models;

public class Establishment : DbEntity
{
    public string Name { get; set; }
    public string Status { get; set; }
    public string CityMunicipality { get; set; }
    public string CompleteAddress { get; set; }
    public string[] Categories { get; set; }
    public string[] Classifications { get; set; }
    public string[] SubClassifications { get; set; }
    public string[] Tags { get; set; }
    
    // Ownership
    public string[] Owners { get; set; }
    public string? OwnershipType { get; set; }
    
    // Contact
    public string ContactPerson { get; set; }
    public string? Designation { get; set; }
    public string? Website { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public Dictionary<string,string> Socials { get; set; }
}