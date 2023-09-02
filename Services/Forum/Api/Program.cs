using Infrastructure;
using System.Text.Json.Serialization;
using Api.Services;
using Application;
using BuildingBlocks.Middleware;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try{
    Log.Information("Starting web host");
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((cbx,lc) =>lc
    .WriteTo.Console()
    );
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
                .ReferenceHandler = ReferenceHandler.Preserve); 
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(op => 
        op.SwaggerEndpoint("/swagger","v1"));
    
}
    
app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<PostService>();
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