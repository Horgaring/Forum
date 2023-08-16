using System.Net;
using System.Text.Json;
using BuildingBlocks.Exception;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace BuildingBlocks.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    

    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (System.Exception exception)
        {
            
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message,
                StatusCode = (exception as CustomExceptions)?.StatusCode ?? HttpStatusCode.BadRequest,
            };
            if (exception is CustomExceptions customException 
                && customException.Messages != null)
            {
                    foreach (var errordetail in customException.Messages)
                    {
                        errorResult.Messages.Add(errordetail);
                    }
            }
            if (exception is FluentValidation.ValidationException fluentException)
            {
                foreach (var error in fluentException.Errors)
                {
                    errorResult.Messages.Add(error.ErrorMessage);
                }
            }
            Log.Error($"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode}");
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)errorResult.StatusCode;
            await response.WriteAsync(JsonSerializer.Serialize(errorResult));
            
            
        }
    }
}
