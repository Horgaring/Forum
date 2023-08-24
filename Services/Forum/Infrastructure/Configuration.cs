using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddDbContext<PostDbContext>(op =>
            op.UseNpgsql(config.GetConnectionString("DefaultConnection")
                ,b => b.MigrationsAssembly("Api")));
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