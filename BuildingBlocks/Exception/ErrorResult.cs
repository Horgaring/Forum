using System.Net;

namespace BuildingBlocks.Exception;

public class ErrorResult
{
    public ErrorResult(string messgaError, string exception, HttpStatusCode statusCode)
    {
        MessgaError = messgaError;
        Exception = exception;
        StatusCode = statusCode;
    }

    public string MessgaError { get; set; }
    public string Exception { get; set; }
    public HttpStatusCode StatusCode { get; set; } 
}