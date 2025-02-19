using Presyotect.Core.Contracts;

namespace Presyotect.Features.Configuration.Models;

public class Category : DbEntity
{
    public string ShortName { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
}