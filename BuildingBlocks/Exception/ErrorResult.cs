using System.Net;
using System.Text.Json;

namespace BuildingBlocks.Exception;

public class ErrorResult
{
    public ErrorResult(string messgaError, string exception, HttpStatusCode statusCode)
    {
        MessgaError = messgaError;
        Exception = exception;
        StatusCode = statusCode;
    }
    public ErrorResult(CustomException exception)
    {
        MessgaError = JsonSerializer.Serialize(exception.Messages);
        Exception = exception.Message;
        StatusCode = exception.code;
    }
    
    public ErrorResult(FluentValidation.ValidationException exception)
    {
        MessgaError = JsonSerializer.Serialize(exception.Errors.Select(p => p.ErrorMessage));
        Exception = exception.Message;
        StatusCode = HttpStatusCode.BadRequest;
    }

    public string MessgaError { get; set; }
    public string Exception { get; set; }
    public HttpStatusCode StatusCode { get; set; } 
}