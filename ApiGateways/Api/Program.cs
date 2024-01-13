using BuildingBlocks.Exception;
using Infrastructure;
using Infrastructure.Clients;
using Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors(op => op.AddPolicy("baseCors", p =>
{
    p.AllowAnyMethod();
    p.AllowAnyHeader();
    p.AllowAnyOrigin();
}));
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("baseCors");
app.UseAuthorization();

app.Run();