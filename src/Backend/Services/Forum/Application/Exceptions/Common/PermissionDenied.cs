using System.Net;
using BuildingBlocks.Exception;

namespace Application.Exceptions.Common;

public class PermissionDenied : CustomException
{
    public PermissionDenied(string[]? messageDetails = null) : base(HttpStatusCode.BadRequest, messageDetails,
        "Permission Denied")
    {
    }
}