using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace Identityserver;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("api"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "React",
                ClientName = "React App",

                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = { new Secret("ee4ecb88-4211-4a10-88e8-9276ab9d1262".Sha256()) },
                
                RedirectUris =           { "http://localhost:3000" },
                PostLogoutRedirectUris = { "http://localhost:3000" },
                AllowedCorsOrigins =     { "http://localhost:3000" },
                
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api",
                }
            },

           
        };
}