namespace Presyotect.Core.Contracts;

public class ResponseData<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string[]? Errors { get; set; }
    public T? Content { get; set; }
    public Pagination? Pagination { get; set; }
}