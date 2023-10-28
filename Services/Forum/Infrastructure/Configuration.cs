using System.Reflection;
using BuildingBlocks;
using BuildingBlocks.Middleware;
using BuildingBlocks.TestBase;
using Grpc.AspNetCore.Server;
using Infrastructure.Context;
using IntegrationTest.Seed;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


namespace Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddBroker(config);
        services.AddScoped<IDataSeeder, PostDataSeeder>();
        services.AddGrpc(option => option.Interceptors.Add<Exceptioninterceptor>());
        services.AddDbContext<PostDbContext>(op =>
            op.UseNpgsql(config.GetConnectionString("DefaultConnection")
                ,b => b.MigrationsAssembly("Api")));
        services.AddSingleton<ISqlconnectionfactory, SqlConnectionFactory>();
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", op =>
            {
                //op.Configuration = new OpenIdConnectConfiguration(); 
                op.RequireHttpsMetadata = false;
                op.Authority = "https://localhost:5001";
                op.TokenValidationParameters = new()
                {
                    ValidateAudience = false
                };
                
            });
        return services;
    }

}