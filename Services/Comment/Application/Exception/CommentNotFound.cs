using System.Net;
using BuildingBlocks.Exception;
using Grpc.Core;

namespace Application.Exception;

public class CommentNotFound: CustomException
{
    public CommentNotFound(StatusCode statusCode, string[]? messages) : base(statusCode,messages, "Comment not found")
    {
    }
}