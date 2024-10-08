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
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            builder.Host.UseSerilog((cbx,lc) =>lc
                .WriteTo.Console()
            );
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddSingleton<ExceptionMiddleware>();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors();
            builder.Services.AddSwaggerGen(p =>
            {
                //p.MapType(typeof(IFormFile), () => new OpenApiSchema() { Type = "file", Format = "binary" });
                p.SwaggerDoc("post", new OpenApiInfo() { Version = "post", Title = "post" });
                p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                p.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,},
                        new List<string>()
                    }
                });
            });
            builder.Services.AddApplication();
            var app = builder.Build();
            if (app.Environment.IsDevelopment()
                || app.Environment.IsEnvironment("Docker"))
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
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(p => p
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.MapEnpoints();
            app.UseSeed<PostDbContext>(app.Environment);
            app.MapHealthChecks("/health");
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