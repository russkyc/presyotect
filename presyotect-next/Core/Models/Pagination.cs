namespace Presyotect.Core.Models;

public class Pagination
{
    public int Page { get; set; }
    public int PageSize { get; init; }
    public int TotalPages { get; set; }
    public int Items { get; set; }
    public int TotalItems { get; set; }
}