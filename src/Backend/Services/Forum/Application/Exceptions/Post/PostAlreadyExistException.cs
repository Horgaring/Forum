using System.Net;
using BuildingBlocks.Exception;

namespace Application.Exceptions.Post;

public class PostAlreadyExistException : CustomException
{
    public PostAlreadyExistException( string[]? messageDetails) 
        : base(HttpStatusCode.BadRequest, messageDetails, "Post with the same title already exists"){}
}