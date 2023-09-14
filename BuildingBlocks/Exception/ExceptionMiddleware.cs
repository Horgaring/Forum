using System.Net;
using System.Text.Json;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace BuildingBlocks.Exception;

public class ExceptionMiddleware: IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (System.Exception e)
        {
            HttpStatusCode code = HttpStatusCode.BadRequest;
            ErrorResult errorResult = new(e.Message, e.TargetSite?.DeclaringType?.FullName ?? "Unknown", code);
            if (e is RpcException ex)
            {
                if (ex.StatusCode == StatusCode.Unauthenticated)
                {
                    code = HttpStatusCode.Unauthorized;
                }else if (ex.StatusCode == StatusCode.NotFound)
                {
                    code = HttpStatusCode.NotFound;
                }
                errorResult = new(ex.Status.Detail, ex.TargetSite?.DeclaringType?.FullName ?? "Unknown", code);
            }
            Log.Error($"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode}");
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)errorResult.StatusCode;
            await response.WriteAsync(JsonSerializer.Serialize(errorResult));
        }
        
    }
}