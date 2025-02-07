using LiteDB;

namespace Presyotect.Core.Models;

public abstract class DbEntity
{
    public ObjectId Id { get; set; } = ObjectId.NewObjectId();
    public DateTime? Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; }
    public DateTime? Deleted { get; set; }
}