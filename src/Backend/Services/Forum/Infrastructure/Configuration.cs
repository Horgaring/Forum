using BuildingBlocks;
using BuildingBlocks.Core.Repository;
using BuildingBlocks.TestBase;
using Infrastructure.Context;
using IntegrationTest.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlocks.Healths;
using Infrastructure.BUS;
using Microsoft.Extensions.Hosting;
using Serilog.Core;
using Infrastructure.Context.Repository;
using Quartz;
using Infrastructure.Seed;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class Configuration
{
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddScoped<IDataSeeder, PostDataSeeder>();
        services.AddSingleton<ConvertDomainToEventOutBoxInterceptor>();
        services.AddDbContext<PostDbContext>((sp,op) =>
            op.UseNpgsql(config.GetConnectionString("DefaultConnection")
                ,b => b.MigrationsAssembly("Api"))
                .AddInterceptors(sp.GetRequiredService<ConvertDomainToEventOutBoxInterceptor>()));
        services.AddTransient<IRepository<Post, Guid>,PostRepository>();
        services.AddTransient<IRepository<Group, Guid>,GroupRepository>();
        services.AddTransient<IRepository<CustomerId, Guid>,CustomerIdRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork<PostDbContext>>();
        services.AddSingleton<ISqlconnectionfactory, SqlConnectionFactory>();
        services.AddScoped<OtboxMesPublishedJob<PostDbContext>>();
        services.AddQuartz(config => {
            config.AddJob<OtboxMesPublishedJob<PostDbContext>>(OtboxMesPublishedJob<PostDbContext>.JobKey)
            .AddTrigger(config =>
            config
                .ForJob(OtboxMesPublishedJob<PostDbContext>.JobKey)
                .WithSimpleSchedule(x => 
                    x.WithInterval(TimeSpan.FromMinutes(1))
                        .RepeatForever()));
        });
        services.AddQuartzHostedService(opt => 
        {
            opt.WaitForJobsToComplete = true; 
        });
        services.AddBroker(config);
        services.AddHealthChecks()
            .AddCheck<SqlHealthCheck>("SqlIsReady");
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", op =>
            {
                //op.Configuration = new OpenIdConnectConfiguration(); 
                if (config["ASPNETCORE_ENVIRONMENT"] == Environments.Development
                    || config["ASPNETCORE_ENVIRONMENT"] == "Docker")
                {
                    op.RequireHttpsMetadata = false;
                    op.TokenValidationParameters.ValidIssuer = "http://localhost:5001";
                }
                op.Audience = "user";
                op.MapInboundClaims = false;
                op.Authority = config["AuthServiceIp"];
            });
        return services;
    }

}