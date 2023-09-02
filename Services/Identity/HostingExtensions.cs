using BuildingBlocks.Middleware;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Identity.Mediatr;
using Identityserver;
using Identityserver.Data;


namespace Identity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        builder.Services.AddCors(option =>
        {
            option.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
           
            builder.Services.AddDefaultIdentity<User>(options =>
                {
                    options.User.AllowedUserNameCharacters = null;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 1; 
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.addCustomMediatR();
        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })

            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<User>();
        
            

        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            });
        builder.Services.AddSingleton<ExceptionMiddleware>();
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseCors();
        app.UseStaticFiles();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionMiddleware>();
        app.MapControllers();
        
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}