using System.Net;
using System.Text.Json;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace BuildingBlocks.Exception;

public class ExceptionMiddleware: IMiddleware
{
    private ErrorResult errorResult;
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (CustomException e)
        {
            errorResult = new(e);

            Log.Error($"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode}");
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)errorResult.StatusCode;
            await response.WriteAsync(JsonSerializer.Serialize(errorResult));
        }
        catch (FluentValidation.ValidationException e)
        {
            errorResult = new(e);
            

            Log.Error($"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode}");
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)errorResult.StatusCode;
            await response.WriteAsync(JsonSerializer.Serialize(errorResult));
        }
        catch (System.Exception e)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError;
            errorResult = new(e.Message, e.TargetSite?.DeclaringType?.FullName ?? "Unknown", code);

            Log.Error($"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode}");
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)errorResult.StatusCode;
            await response.WriteAsync(JsonSerializer.Serialize(errorResult));
        }
        
        
    }
}