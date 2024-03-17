using BuildingBlocks;
using BuildingBlocks.Core.Repository;

using BuildingBlocks.TestBase;
using Infrastructure.Context;
using IntegrationTest.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlocks.Core.Repository;
using BuildingBlocks.Healths;
using Infrastructure.BUS;
using Microsoft.Extensions.Hosting;
using Serilog.Core;

namespace Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddScoped<IDataSeeder, PostDataSeeder>();
        services.AddDbContext<PostDbContext>(op =>
            op.UseNpgsql(config.GetConnectionString("DefaultConnection")
                ,b => b.MigrationsAssembly("Api")));
        services.AddTransient<PostRepository,PostRepository>();
        services.AddTransient<GroupRepository,GroupRepository>();
        services.AddTransient<CustomerIdRepository,CustomerIdRepository>();
        services.AddScoped<IUnitOfWork<PostDbContext>, UnitOfWork<PostDbContext>>();
        services.AddSingleton<ISqlconnectionfactory, SqlConnectionFactory>();
        services.AddBroker(config);
        services.AddHealthChecks()
            .AddCheck<SqlHealthCheck>("SqlIsReady");
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(  "Bearer", op =>
            {
                //op.Configuration = new OpenIdConnectConfiguration(); 
                if (config["ASPNETCORE_ENVIRONMENT"] == Environments.Development)
                {
                    
                    op.RequireHttpsMetadata = false;
                    op.TokenValidationParameters = new()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                    
                }

                op.MapInboundClaims = false;
                op.Authority = config["AuthServiceIp"];
                
            });
        return services;
    }

}