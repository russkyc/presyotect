using Presyotect.Core.Contracts;

namespace Presyotect.Features.Products.Models;

public class Product : DbEntity
{
    public string? Sku { get; set; }
    public string Name { get; set; }
    public string? Size { get; set; }
    public string? Status { get; set; }
    public string[]? Category { get; set; }
    public string Classification { get; set; }
    public decimal? Srp { get; set; }
}