using System.Net;
using BuildingBlocks.Exception;

namespace Application.Exceptions.Common;

public class CustomerNotFound: CustomException
{
    public CustomerNotFound( string[]? messageDetails = null) : base(HttpStatusCode.BadRequest, messageDetails, "Customer Not Fount")
    {
    }
}