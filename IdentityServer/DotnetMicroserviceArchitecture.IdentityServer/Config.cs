using IdentityServer4;
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
                     new ApiResource("resource_stock"){ Scopes={ "stock_fullpermission" } },
                     new ApiResource("resource_basket"){ Scopes={ "basket_fullpermission" } },
                     new ApiResource("resource_discount"){ Scopes={ "discount_fullpermission" } },
                     new ApiResource("resource_order"){ Scopes={ "order_fullpermission" } },
                     new ApiResource("resource_payment"){ Scopes={ "payment_fullpermission" } },
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
                       new ApiScope("stock_fullpermission","Stock API"),
                       new ApiScope("basket_fullpermission","Basket API"),
                       new ApiScope("discount_fullpermission","Discount API"),
                       new ApiScope("order_fullpermission","Order API"),
                       new ApiScope("payment_fullpermission","Payment API"),
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

                          AllowedScopes = { "catalog_fullpermission", "stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
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
                              "order_fullpermission",
                              "discount_fullpermission",
                              "basket_fullpermission",
                              "payment_fullpermission",
                              IdentityServerConstants.StandardScopes.Email,
                              IdentityServerConstants.StandardScopes.OpenId,
                              IdentityServerConstants.StandardScopes.Profile,
                              IdentityServerConstants.StandardScopes.OfflineAccess, // offline halde token almak için gerekli
                              IdentityServerConstants.LocalApi.ScopeName,
                              "roles"
                          },
                          AccessTokenLifetime = 1*60*60,
                          RefreshTokenExpiration = TokenExpiration.Absolute,
                          AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(30) - DateTime.Now).TotalSeconds,
                          RefreshTokenUsage = TokenUsage.ReUse //reflesh token tekrar kullanılabilsin
                      }
                };
    }
}