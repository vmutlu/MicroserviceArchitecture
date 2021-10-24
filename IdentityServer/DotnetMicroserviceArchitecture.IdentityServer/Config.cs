﻿using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
         new ApiResource[]
                 {
                     new ApiResource("resource_catalog"){ Scopes={"catalog_fullpermission"} },
                     new ApiResource("resource_photo_stock"){ Scopes={ "photo_stock_fullpermission" } },
                     new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
                 };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){ Name = "roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims = new[]{ "role" } },
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
                   {
                       new ApiScope("catalog_fullpermission","Catalog API"),
                       new ApiScope("photo_stock_fullpermission","Photo Stock API"),
                       new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
                   };

        public static IEnumerable<Client> Clients =>
                new Client[]
                {
                      // m2m client credentials flow client
                      new Client
                      {
                          ClientId = "web.client",
                          ClientName = "Client Credentials Client",

                          AllowedGrantTypes = GrantTypes.ClientCredentials, //reflesh token olmayacak
                          ClientSecrets = { new Secret("secret".Sha256()) },

                          AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                      },

                      new Client
                      {
                          ClientId = "client.user",
                          ClientName = "Client Credentials Client For User",

                          AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                          ClientSecrets = { new Secret("secret".Sha256()) },
                          AllowOfflineAccess = true, // offline özelliği kullanabilmek için açıldı
                          AllowedScopes =
                          {
                              IdentityServerConstants.StandardScopes.Email,
                              IdentityServerConstants.StandardScopes.OpenId,
                              IdentityServerConstants.StandardScopes.Address,
                              IdentityServerConstants.StandardScopes.Profile,
                              IdentityServerConstants.StandardScopes.OfflineAccess, // offline halde token almak için gerekli
                              IdentityServerConstants.StandardScopes.Phone,
                              IdentityServerConstants.LocalApi.ScopeName,
                              "roles"
                          },
                          AccessTokenLifetime = (int)(DateTime.Now.AddHours(1) - DateTime.Now).TotalSeconds,
                          RefreshTokenExpiration = TokenExpiration.Absolute,
                          AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(30) - DateTime.Now).TotalSeconds,
                          RefreshTokenUsage = TokenUsage.ReUse //reflesh token tekrar kullanılabilsin
                      }
                };
    }
}