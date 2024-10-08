
using Api.Endpoints;
using BuildingBlocks.Exception;
using BuildingBlocks.Extension;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Configuration.AddEnvironmentVariables();
    builder.Host.UseSerilog((cbx,lc) =>lc
        .WriteTo.Console()
    );
    builder.Services.AddSingleton<ExceptionMiddleware>();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddSwaggerGen(p =>
    {
        p.SwaggerDoc("identity", new OpenApiInfo() { Version = "identity", Title = "identity" });
        p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
    var app = builder.Build();

    if (app.Environment.IsDevelopment()
        || app.Environment.IsEnvironment("Docker"))
    {
        IdentityModelEventSource.ShowPII = true;
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/identity/swagger.json", "identity"); });
    }

    app.UseForwardedHeaders();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapEnpoints();
    app.UseSeed<ApplicationDbContext>(app.Environment);
    app.MapHealthChecks("/health");
    app.UseSerilogRequestLogging();
    app.Run();
}
catch (Exception ex){
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally{
    Log.CloseAndFlush();
}