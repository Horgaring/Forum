using System.Net;
using BuildingBlocks.Exception;

namespace Application.Exceptions.Post;

public class PostNotFountException : CustomException
{
    public PostNotFountException( string[]? messageDetails) : base(HttpStatusCode.BadRequest, messageDetails, "Post Not Fount")
    {
    }
}