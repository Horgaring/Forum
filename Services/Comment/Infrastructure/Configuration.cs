using System.Collections.Immutable;
using BuildingBlocks;
using BuildingBlocks.Core.Repository;
using BuildingBlocks.Extension;
using BuildingBlocks.Middleware;
using BuildingBlocks.TestBase;
using Infrastructure.Bus;
using Infrastructure.Context;
using Infrastructure.Seed;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBroker(configuration);
        services.AddScoped<IDataSeeder, CommentDataSeeder>();
        services.AddGrpc(option => option.Interceptors.Add<Exceptioninterceptor>());
        services.AddDbContext<CommentDbContext>(op =>
            op.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddTransient<CommentRepository,CommentRepository>();
        services.AddTransient<IUnitOfWork<CommentDbContext>, UnitOfWork<CommentDbContext>>();
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