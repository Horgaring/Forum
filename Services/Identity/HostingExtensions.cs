using BuildingBlocks.Middleware;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Identity.Mediatr;
using Identityserver;
using Identityserver.Data;
using Identityserver.Exceptions;
using Identityserver.Models;
using Identityserver.Models.Store;
using Identityserver.Services;


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
        builder.Services.AddTransient<IProfileService, ProfileService>();
        builder.Services.AddScoped<IImageStore, FileSystemStore>();
        builder.Services.AddScoped<IImageService, ImageService>();
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
            .AddAspNetIdentity<User>()
            .AddProfileService<ProfileService>();
            

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
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSerilogRequestLogging();
        app.UseCors();
        app.UseStaticFiles();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.MapControllers();
        
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}