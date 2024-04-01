using System.Collections.Immutable;
using BuildingBlocks;
using BuildingBlocks.Core.Repository;
using BuildingBlocks.Extension;
using BuildingBlocks.Healths;
using BuildingBlocks.TestBase;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Bus;
using Infrastructure.Context;
using Infrastructure.Seed;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace Infrastructure.Configurations;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDataSeeder, CommentDataSeeder>();
        services.AddSingleton<ConvertDomainToEventOutBoxInterceptor>();
        services.AddDbContext<CommentDbContext>((sp,op) => 
            
            op.UseNpgsql(configuration.GetConnectionString("DefaultConnection")
                ,b => b.MigrationsAssembly("Api"))
                .AddInterceptors(sp.GetRequiredService<ConvertDomainToEventOutBoxInterceptor>()));
        services.AddScoped<CommentRepository,CommentRepository>();
        services.AddScoped<IUnitOfWork<CommentDbContext>, UnitOfWork<CommentDbContext>>();
        services.AddSingleton<ISqlconnectionfactory, SqlConnectionFactory>();
        services.AddBroker(configuration);
        services.AddHealthChecks()
            .AddCheck<SqlHealthCheck>("SqlIsReady");
        services.AddAuthorization();
        services.AddScoped<OtboxMesPublishedJob<CommentDbContext>>();
        services.AddQuartz(config => {
            config.AddJob<OtboxMesPublishedJob<CommentDbContext>>(OtboxMesPublishedJob<CommentDbContext>.JobKey);
        });
        services.AddQuartzHostedService(opt => 
        {
            opt.WaitForJobsToComplete = true; 
        });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", op =>
            {
                if (configuration["ASPNETCORE_ENVIRONMENT"] == Environments.Development)
                {
                    op.RequireHttpsMetadata = false;
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        //ValidateAudience = false
                        
                    };
                }
                //http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
                op.MapInboundClaims = false;
                op.Audience = "user";
                op.Authority = configuration["AuthServiceIp"];
                
            });
        return services;
    }
    
    

    
    
}