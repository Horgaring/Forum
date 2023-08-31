using System.Net;
using BuildingBlocks.Exception;

namespace Application.Exception;

public class CommentNotFound: CustomExceptions
{
    public CommentNotFound(HttpStatusCode statusCode, string[]? messages) : base("Comment not found", statusCode, messages)
    {
    }
}