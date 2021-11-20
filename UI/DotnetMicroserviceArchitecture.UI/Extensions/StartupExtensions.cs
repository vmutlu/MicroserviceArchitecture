using DotnetMicroserviceArchitecture.Core.Services.Concrete;
using DotnetMicroserviceArchitecture.UI.Handler;
using DotnetMicroserviceArchitecture.UI.Helpers;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using DotnetMicroserviceArchitecture.UI.Services.Concrete;
using DotnetMicroserviceArchitecture.UI.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotnetMicroserviceArchitecture.UI.Extensions
{
    public static class StartupExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpClient<Services.Abstract.IIdentityService, Services.Concrete.IdentityService>();

            var apiSettings = Configuration.GetSection("ApiSettings").Get<ApiSettings>();

            //category service implementation
            services.AddHttpClient<ICatalogService, CatalogService>(options =>
            {
                options.BaseAddress = new Uri($"{ apiSettings.GatewayURL }/{apiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<TokenHandler>();

            //stock service implementation
            services.AddHttpClient<IStockService, StockService>(options =>
            {
                options.BaseAddress = new Uri($"{ apiSettings.GatewayURL }/{apiSettings.Stock.Path}");
            }).AddHttpMessageHandler<TokenHandler>();

            //basket service implementation
            services.AddHttpClient<IBasketService, BasketService>(options =>
            {
                options.BaseAddress = new Uri($"{ apiSettings.GatewayURL }/{apiSettings.Basket.Path}");
            }).AddHttpMessageHandler<TokenHandler>();

            services.AddHttpClient<ITokenService, TokenService>();

            services.AddSingleton<PhotoURLEditHelper>();

            services.AddAccessTokenManagement();

            services.AddScoped<ResourceOwnerTokenHandler>();

            services.AddScoped<TokenHandler>();

            services.AddScoped<Core.Services.Abstract.IIdentityService, Core.Services.Concrete.IdentityService>();

            services.AddHttpContextAccessor();

            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));

            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));

            services.AddHttpClient<IUserService, UserService>(options =>
            {
                options.BaseAddress = new Uri(apiSettings.IdentityURL);
            }).AddHttpMessageHandler<ResourceOwnerTokenHandler>();//her istekte headere token bilgisi eklemesi için

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Auth/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                options.SlidingExpiration = true; options.Cookie.Name = "microserviceprojectcookie";
            });

        }
    }
}
