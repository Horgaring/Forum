using Application.PostRequests;
using BuildingBlocks.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Configure
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePostRequest).Assembly));
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return service;
    }
}