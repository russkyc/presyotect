
using System.ComponentModel.DataAnnotations;

namespace Presyotect.Core.Contracts;

public abstract class DbEntity
{
    [Key]
    public Guid Id { get; set; }
    public string? Key { get; set; }
    public DateTime? Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; }
    public DateTime? Deleted { get; set; }
    public string Organization { get; set; }
}