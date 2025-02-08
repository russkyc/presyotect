using System.Text.Json.Serialization;
using LiteDB;
using Presyotect.Utilities;

namespace Presyotect.Core.Models;

public abstract class DbEntity
{
    [JsonConverter(typeof(ObjectIdConverter))]
    public ObjectId Id { get; set; }

    public string? Key { get; set; }
    public DateTime? Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; }
    public DateTime? Deleted { get; set; }
}