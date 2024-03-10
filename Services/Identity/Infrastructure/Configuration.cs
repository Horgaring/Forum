
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
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddBroker(config);
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", op =>
            {
                if (config["ASPNETCORE_ENVIRONMENT"] == Environments.Development)
                {
                    op.RequireHttpsMetadata = false;
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                }
                //op.Configuration = new OpenIdConnectConfiguration(); 
                op.Authority = config["AuthServiceIp"];
                
            });
        return services;
    }

}

