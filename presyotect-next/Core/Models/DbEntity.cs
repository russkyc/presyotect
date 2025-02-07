using LiteDB;

namespace Presyotect.Core.Models;

public abstract class DbEntity
{
    public int Id { get; set; }
    public DateTime? Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; }
    public DateTime? Deleted { get; set; }
}