using BuildingBlocks.TestBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace BuildingBlocks.Extension;

public static class ConfigurationExtensions
{
    public static TModel GetOptions<TModel>(this IConfiguration configuration) where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(nameof(model)).Bind(model);
        return model;
    }

    public static TModel GetOptions<TModel>(this IServiceCollection service) where TModel : new()
    {
        var model = new TModel();
        var configuration = service.BuildServiceProvider().GetService<IConfiguration>();
        configuration?.GetSection(nameof(model)).Bind(model);
        return model;
    }

    public static TModel GetOptions<TModel>(this WebApplication app) where TModel : new()
    {
        var model = new TModel();
        app.Configuration?.GetSection(nameof(model)).Bind(model);
        return model;
    }

    public static void AddOption<TModel>(this IServiceCollection service) where TModel : class, new()
    {
        service.AddOptions<TModel>();
        service.AddSingleton(x => x.GetRequiredService<IOptions<TModel>>().Value);
    }
    
    public async  static Task<IApplicationBuilder> UseSeed<TContext>(this IApplicationBuilder app, IWebHostEnvironment env)
        where TContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TContext>();
        Log.Information("Migrate Start");
        db.Database.Migrate();
        if (env.IsDevelopment())
        {
            
            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>().ToList();
            foreach (var seeder in seeders)
            {
                await seeder.SeedAllAsync();
            }
        }
        return app;
    }
    
}