using BuildingBlocks;
using BuildingBlocks.Middleware;
using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IDataSeeder, CommentDataSeeder>();
        services.AddGrpc(option => option.Interceptors.Add<Exceptioninterceptor>());
        services.AddDbContext<CommentDbContext>(op =>
            op.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        services.AddSingleton<ISqlconnectionfactory, SqlConnectionFactory>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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