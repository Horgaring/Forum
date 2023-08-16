using System.Net;

namespace BuildingBlocks.Exception;

public class ErrorResult
{
    public List<string> Messages { get; set; } = new();

    public string? Source { get; set; }
    public string? Exception { get; set; }
    public HttpStatusCode StatusCode { get; set; } 
}