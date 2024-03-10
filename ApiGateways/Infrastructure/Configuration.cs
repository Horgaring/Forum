using BuildingBlocks;
using BuildingBlocks.Extension;
using Infrastructure.Clients;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration cfg)
    {
        service.AddOption<CommentGrpcOption>();
        service.AddOption<PostGrpcOption>();
        service.AddGrpcServices();
        service.AddAuthorization();
        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        return service;
    }
}