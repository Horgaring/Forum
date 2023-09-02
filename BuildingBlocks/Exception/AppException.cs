using Grpc.Core;

namespace BuildingBlocks.Exception;

public class CustomException: System.Exception
{
    public CustomException(StatusCode code, string[]? messages, string message)
    {
        this.code = code;
        Messages = messages;
        Message = message;
    }

    public StatusCode code { get; init; }
    public string[]? Messages  { get; init; }
    public string Message  { get; init; }
}