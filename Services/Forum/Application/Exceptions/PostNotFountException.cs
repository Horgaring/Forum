using System.Net;
using BuildingBlocks.Exception;
using Grpc.Core;

namespace Application.Exceptions;

public class PostNotFountException : CustomException
{
    public PostNotFountException( StatusCode statusCode, string[]? messageDetails) : base(statusCode, messageDetails, "Post Not Fount")
    {
    }
}