using System.Net;
using BuildingBlocks.Exception;
using Grpc.Core;

namespace Application.Exception;

public class CommentNotFound: CustomException
{
    public CommentNotFound(string[]? messages) : base(HttpStatusCode.BadRequest,messages, "Comment not found")
    {
    }
}