using System.Net;
using BuildingBlocks.Exception;

namespace Application.Exceptions.Group;

public class GroupNotFoundExeption : CustomException
{
    public GroupNotFoundExeption( string[]? messageDetails = null) : base(HttpStatusCode.BadRequest, messageDetails, "Group Not Fount")
    {
    }
}