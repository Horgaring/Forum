using BuildingBlocks.Exception;
using Grpc.Core;

namespace Application.Exceptions;

public class PostAlreadyExistException : CustomException
{
    public PostAlreadyExistException( StatusCode statusCode, string[]? messageDetails) 
        : base(statusCode, messageDetails, "Post with the same title already exists"){}
}