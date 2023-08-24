using System.Net;
using BuildingBlocks.Exception;

namespace Identityserver.Exceptions;

public class RegisterUserException: CustomExceptions 
{
    public RegisterUserException(string message, HttpStatusCode statusCode,string[] errordetail) : base(message, statusCode,errordetail)
    {
    }
}