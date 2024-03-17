
using Infrastructure.BUS;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service,IConfiguration conf)
    {
        service.AddBroker(conf);
        return service;
    }
    public static void UseInfrastructure(this WebApplication service)
    {
        
    }
}