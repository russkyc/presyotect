using System.Text.Json.Serialization;
using LiteDB;
using Presyotect.Core.Contracts;
using Presyotect.Utilities;

namespace Presyotect.Features.Monitoring.Models;

public class MonitoredEstablishment : DbEntity
{
    public string Name { get; set; }
    public string CityMunicipality { get; set; }
    public string CompleteAddress { get; set; }
    public string[] Categories { get; set; }
    public string[] Classifications { get; set; }
    public string[] SubClassifications { get; set; }
}