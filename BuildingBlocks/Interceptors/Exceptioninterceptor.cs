using System.Net;
using System.Text;
using System.Text.Json;
using BuildingBlocks.Exception;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Serilog;

namespace BuildingBlocks.Middleware;

public class Exceptioninterceptor: Interceptor
{

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (System.Exception exception)
        {
            StringBuilder sb = new();
            StatusCode statuscode = StatusCode.Unknown;
            var customexception = exception as CustomException;
            if (customexception != null)
            {
                statuscode = customexception.code;
            }
            if (exception is CustomException customException 
                && customException.Messages != null)
            {
                
                foreach (var errordetail in customException.Messages)
                {
                    sb.Append(errordetail);
                }
            }
            if (exception is FluentValidation.ValidationException fluentException)
            {
                statuscode = StatusCode.InvalidArgument;
                foreach (var error in fluentException.Errors)
                {
                    sb.Append(error.ErrorMessage + "\n");
                }
            }
            Status status = new Status(statuscode, sb.ToString());
            Log.Error($"{status.Detail} Request failed with Status Code {status.StatusCode}");
            throw new RpcException(status);
        }
    }
}