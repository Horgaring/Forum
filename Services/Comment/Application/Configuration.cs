using Application.Mapping;
using BuildingBlocks.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Configuration
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddAutoMapper(typeof(MappingProfile));
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Configuration).Assembly));
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return service;
    }
}