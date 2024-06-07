using BuildingBlocks.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog(p => 
        p.WriteTo.Console());
    builder.Services.AddSingleton<ExceptionMiddleware>();
    builder.Services.AddCors(p=> 
        p.AddPolicy("REACTAPP", builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("Yarp"))
        .AddSwagger(builder.Configuration.GetSection("Yarp"));
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var config = app.Services.GetRequiredService<IOptionsMonitor<ReverseProxyDocumentFilterConfig>>().CurrentValue;
            foreach (var cluster in config.Clusters)
            {
                options.SwaggerEndpoint($"/swagger/{cluster.Key}/swagger.json", cluster.Key);
            }
        });
    }
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseCors("REACTAPP");
    app.UseForwardedHeaders();
    app.UseSerilogRequestLogging();
    app.MapReverseProxy();
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