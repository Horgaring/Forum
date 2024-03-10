using Api.Endpoints;
using Application;
using BuildingBlocks.Core.Events.Post;
using BuildingBlocks.Exception;
using BuildingBlocks.Extension;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Context;
using Mapster;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        try{
            Log.Information("Starting web host");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog((cbx,lc) =>lc
                .WriteTo.Console()
            );
            IdentityModelEventSource.ShowPII = true;
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddSingleton<ExceptionMiddleware>();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("post", new OpenApiInfo() { Version = "post", Title = "post" });
                p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
            });
            builder.Services.AddApplication();
            // builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
            //     .ReferenceHandler = ReferenceHandler.Preserve); 
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/post/swagger.json", "post");
                });
            }
            app.UseForwardedHeaders();
            app.UseMiddleware<ExceptionMiddleware>();
            app.MapEnpoints();
            app.UseSeed<PostDbContext>(app.Environment);
            app.MapHealthChecks("/health");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.Run();
        }
        catch (Exception ex){
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally{
            Log.CloseAndFlush();
        }
    }
}