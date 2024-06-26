﻿using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using Identityserver.Constants;

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
            new(IsConstants.StandardScopes.IdentityApi),
            new(IsConstants.StandardScopes.CommentApi),
            new(IsConstants.StandardScopes.PostApi),
        };
    
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new("user")
            {
                Scopes =
                {
                    IsConstants.StandardScopes.IdentityApi,
                    IsConstants.StandardScopes.CommentApi,
                    IsConstants.StandardScopes.PostApi
                }
            },
            
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
                AllowOfflineAccess = true,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                    IsConstants.StandardScopes.IdentityApi,
                    IsConstants.StandardScopes.PostApi,
                    IsConstants.StandardScopes.CommentApi
                }
            },

           
        };

    
}