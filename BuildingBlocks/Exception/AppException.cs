using System.Net;
using Grpc.Core;

namespace BuildingBlocks.Exception;

public class CustomException: System.Exception
{
    public CustomException(HttpStatusCode code, string[]? messages, string message)
    {
        this.code = code;
        Messages = messages;
        Message = message;
    }

    public HttpStatusCode code { get; init; }
    public string[]? Messages  { get; init; }
    public string Message  { get; init; }
}