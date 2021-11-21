using DotnetMicroserviceArchitecture.UI.Extensions;
using DotnetMicroserviceArchitecture.UI.Handler;
using DotnetMicroserviceArchitecture.UI.Helpers;
using DotnetMicroserviceArchitecture.UI.Services.Abstract;
using DotnetMicroserviceArchitecture.UI.Services.Concrete;
using DotnetMicroserviceArchitecture.UI.Settings;
using DotnetMicroserviceArchitecture.UI.Validators;
using FluentValidation.AspNetCore;
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
            services.AddServices(Configuration);

            //RegisterValidatorsFromAssemblyContaining validator classlarýný tarar ve validate classlarýný çalýþtýrýr
            services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CourseCreateRequestValidator>());
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
