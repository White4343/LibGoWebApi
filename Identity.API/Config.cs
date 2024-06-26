﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;

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
            new ApiScope("bookgenres.admin", "Admin BookGenres API"),
            new ApiScope("bookgenres.client", "Client BookGenres API"),
            new ApiScope("books.client", "Client Books API"),
            new ApiScope("books.admin", "Admin Books API"),
            new ApiScope("comments.client", "Client Comments API"),
            new ApiScope("comments.admin", "Admin Comments API"),
            new ApiScope("chapters.client", "Client Chapters API"),
            new ApiScope("chapters.admin", "Admin Chapters API"),
            new ApiScope("users.client", "Client Users API"),
            new ApiScope("users.admin", "Admin Users API"),
            new ApiScope("readers.client", "Client Readers API"),
            new ApiScope("readers.admin", "Admin Readers API"),
            new ApiScope("nickname", "User nickname", userClaims: new []{"nickname"}),
            new ApiScope("role", "User role", userClaims: new []{"role"}),
            new ApiScope("photoUrl", "User Photo Url", userClaims: new []{"photoUrl"}),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "booksswaggerui",
                ClientName = "Books Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,

                RedirectUris = {WebApiLinks.BooksApi + "/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = {WebApiLinks.BooksApi + "/swagger/" },

                AllowedScopes =
                {
                    "books.admin",
                    "books.client",
                    "bookgenres.admin",
                    "bookgenres.client",                    
                    "genres.admin",
                    "genres.client",
                    "comments.client",
                    "comments.admin",
                    "readers.client",
                    "readers.admin",
                    "nickname",
                    "role",
                    "photoUrl"
                }
            },
            new Client
            {
                ClientId = "chapterswaggerui",
                ClientName = "Chapter Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = {WebApiLinks.ChapterApi + "/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = {WebApiLinks.ChapterApi + "/swagger/" },

                AllowedScopes =
                {
                    "chapters.admin",
                    "chapters.client"
                }
            },
            new Client
            {
                ClientId = "usersswaggerui",
                ClientName = "Users Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = {WebApiLinks.UsersApi + "/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = {WebApiLinks.UsersApi + "/swagger/" },

                AllowedScopes =
                {
                    "users.admin",
                    "users.client",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
            new Client
            {
                ClientId = "userweb",
                ClientName = "User Web",
                AllowAccessTokensViaBrowser = true,
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                AllowedScopes =
                {
                    "users.client",
                    "chapters.client",
                    "bookgenres.client",
                    "comments.client",
                    "books.client",
                    "genres.client",
                    "readers.client",
                    "nickname",
                    "role",
                    "photoUrl",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        };
}