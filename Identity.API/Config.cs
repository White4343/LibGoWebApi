﻿using Duende.IdentityServer.Models;

namespace Identity.API;

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
            new ApiScope("genres.admin", "Admin Genres API"),
            new ApiScope("genres.client", "Client Genres API"),
            new ApiScope("bookgenres.admin", "admin BookGenres API"),
            new ApiScope("bookgenres.client", "Client BookGenres API"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "genresswaggerui",
                ClientName = "Genres Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = {WebApiLinks.GenresApi + "/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = {WebApiLinks.GenresApi + "/swagger/" },

                AllowedScopes =
                {
                    "genres.admin",
                    "genres.client",
                    "bookgenres.admin",
                    "bookgenres.client"
                }
            }
        };
}
