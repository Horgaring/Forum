using System.Net;

namespace BuildingBlocks.Exception;

public abstract class CustomExceptions: System.Exception
{
    public HttpStatusCode StatusCode { get; }
    public string[]? Messages { get; }
    public CustomExceptions(string message
        ,HttpStatusCode statusCode,string[]? messages) : base(message)
    {
        StatusCode = statusCode;
        Messages = messages;
    }
}