using BuildingBlocks.Exception;
using Infrastructure;
using Infrastructure.Clients;
using Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();