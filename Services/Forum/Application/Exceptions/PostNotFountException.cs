using System.Net;
using BuildingBlocks.Exception;

namespace Application.Exceptions;

public class PostNotFountException : CustomExceptions
{
    public PostNotFountException( HttpStatusCode statusCode, string[]? messageDetails) : base("Post Not Fount", statusCode, messageDetails)
    {
    }
}