
using Domain;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;


namespace Infrastructure;

public static class Configuration
{
    public static  IServiceCollection AddInfrastructure(this  IServiceCollection services, IConfiguration config)
    {
        
        services.AddDbContext<ApplicationDbContext>(op =>
            op.UseNpgsql(config.GetConnectionString("DefaultConnection")
                , b => b.MigrationsAssembly("Api")));
        services.AddBroker(config);
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", op =>
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
                op.Audience = "user";
                op.MapInboundClaims = false;
                op.Authority = config["AuthServiceIp"];
                
            });
        return services;
    }

}

