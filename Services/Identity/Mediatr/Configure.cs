using BuildingBlocks.Validation;
using MediatR;

namespace Identity.Mediatr;

public static class Configure
{
    public static IServiceCollection addCustomMediatR(this IServiceCollection service)
    {
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return service;
    }
}