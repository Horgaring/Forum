using Api.Services;
using BuildingBlocks.Extension;
using Infrastructure.Configurations;
using Infrastructure.Context;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddInfrastructure(builder.Configuration);
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapGrpcService<CommentService>();
        app.UseHttpsRedirection();
        app.MapHealthChecks("/health");
        app.UseSeed<CommentDbContext>(app.Environment);

        app.MapControllers();

        app.Run();
    }
}