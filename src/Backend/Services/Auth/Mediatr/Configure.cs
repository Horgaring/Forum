using BuildingBlocks.Validation;
using MediatR;

namespace Identity.Mediatr;

public static class Configure
{
    public static IServiceCollection addCustomMediatR(this IServiceCollection service)
    {
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            return service;
    }
}