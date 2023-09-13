using BuildingBlocks;
using BuildingBlocks.Extension;
using Infrastructure.Clients;
using Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration cfg)
    {
        service.AddOption<CommentGrpcOption>();
        service.AddOption<PostGrpcOption>();
        service.AddGrpcServices();
        return service;
    }
}