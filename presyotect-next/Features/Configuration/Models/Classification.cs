using Presyotect.Core.Contracts;

namespace Presyotect.Features.Configuration.Models;

public class Classification : DbEntity
{
    public string Name { get; set; }
    public string ShortName { get; set; }
}