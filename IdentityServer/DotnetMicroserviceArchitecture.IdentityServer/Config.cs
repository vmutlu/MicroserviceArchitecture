﻿using IdentityServer4;
using IdentityServer4.Models;
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

                          AllowedGrantTypes = GrantTypes.ClientCredentials,
                          ClientSecrets = { new Secret("secret".Sha256()) },

                          AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                      }
                };
    }
}