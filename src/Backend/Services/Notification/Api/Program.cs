using Infrastructure;
using Infrastructure.Hub;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", op =>
    {
        op.Authority = "https:localhost:5001";
        op.TokenValidationParameters = new()
        {
            ValidateAudience = false
        };
        op.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var endpoint = context.HttpContext.GetEndpoint();
                if (endpoint?.Metadata.GetMetadata<HubMetadata>() is null)
                {
                    return Task.CompletedTask;
                }
                context.Token = accessToken;

                return Task.CompletedTask;
            }
        };
    });
var app = builder.Build();

app.UseInfrastructure();

app.MapHub<NotificationsHub>("/hub/notificationhub");



await app.RunAsync();