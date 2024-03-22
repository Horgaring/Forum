
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
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddSwaggerGen(p =>
    {
        p.SwaggerDoc("identity", new OpenApiInfo() { Version = "identity", Title = "identity" });
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
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        IdentityModelEventSource.ShowPII = true;
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/identity/swagger.json", "identity"); });
    }

    app.UseForwardedHeaders();
    app.UseHttpsRedirection();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthentication();
    app.UseRouting();
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