using Api.Services;
using BuildingBlocks.Extension;
using BuildingBlocks.Middleware;
using Infrastructure.Configurations;
using Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


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
        app.UseSeed<CommentDbContext>(app.Environment);

        app.MapControllers();

        app.Run();
    }
}