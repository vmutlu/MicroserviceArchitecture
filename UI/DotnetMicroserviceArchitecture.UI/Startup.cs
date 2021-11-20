using DotnetMicroserviceArchitecture.UI.Handler;
using DotnetMicroserviceArchitecture.UI.Helpers;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using DotnetMicroserviceArchitecture.UI.Services.Concrete;
using DotnetMicroserviceArchitecture.UI.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DotnetMicroserviceArchitecture.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => (Configuration) = (configuration);

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IIdentityService, IdentityService>();

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

            services.AddHttpClient<ITokenService, TokenService>();

            services.AddSingleton<PhotoURLEditHelper>();

            services.AddAccessTokenManagement();

            services.AddScoped<ResourceOwnerTokenHandler>();

            services.AddScoped<TokenHandler>();

            services.AddScoped<DotnetMicroserviceArchitecture.Core.Services.Abstract.IIdentityService, DotnetMicroserviceArchitecture.Core.Services.Concrete.IdentityService>();

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

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
