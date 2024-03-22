using Api.Endpoints;
using Application;
using BuildingBlocks.Exception;
using BuildingBlocks.Extension;
using Infrastructure.Configurations;
using Infrastructure.Context;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            builder.Host.UseSerilog((cbx,lc) =>lc
                .WriteTo.Console()
            );
            builder.Services.AddSingleton<ExceptionMiddleware>();
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("comment", new OpenApiInfo() { Version = "comment", Title = "comment" });
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
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/comment/swagger.json", "Comment"); });
            }

            app.UseForwardedHeaders();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.MapHealthChecks("/health");
            app.UseSeed<CommentDbContext>(app.Environment);
            app.MapEnpoints();
            app.UseSerilogRequestLogging();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}