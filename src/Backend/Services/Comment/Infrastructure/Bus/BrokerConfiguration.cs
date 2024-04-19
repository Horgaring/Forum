using MassTransit;
using MassTransit.RabbitMqTransport.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Bus;

public static class BrokerConfiguration
{
    public static void AddBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PostDeletedConsumer>();
        
        services.AddMassTransit(cnf =>
        {
            cnf.SetKebabCaseEndpointNameFormatter();
            
            cnf.AddConsumer<PostDeletedConsumer>();
            
            cnf.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:UserName"]);
                    h.Password(configuration["MessageBroker:Password"]);
                });
                configurator.ConfigureEndpoints(context);
            });
        });
    }
}