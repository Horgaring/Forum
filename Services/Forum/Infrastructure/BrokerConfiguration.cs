using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class BrokerConfiguration
{
    public static void AddBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(cnf =>
        {
            cnf.SetKebabCaseEndpointNameFormatter();
            
            cnf.UsingRabbitMq((context, configurator) =>
            {
                 configurator.Host(new Uri(configuration["MessageBroker:Host"]), h =>
                 {
                     h.Username(configuration["MessageBroker-UserName"]);
                     h.Password(configuration["MessageBroker-Password"]);
                 });
                
                configurator.ConfigureEndpoints(context);
            });
        });
    }
}