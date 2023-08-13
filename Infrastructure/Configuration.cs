using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(op =>
            op.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", op =>
            {
                op.Authority = "https:localhost:5001";
                op.TokenValidationParameters = new()
                {
                    ValidateAudience = false
                };
            });
        return services;
    }

}