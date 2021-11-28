using DotnetMicroserviceArchitecture.CatalogAPI.Services.Abstract;
using DotnetMicroserviceArchitecture.CatalogAPI.Services.Concrete;
using DotnetMicroserviceArchitecture.CatalogAPI.Settings.Abstract;
using DotnetMicroserviceArchitecture.CatalogAPI.Settings.Concrete;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Extensions
{
    public static class StartupExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            //configure masstransit
            services.AddMassTransit(ms =>
            {
                //port: 5672
                //uı screen: 15672
                ms.UsingRabbitMq((context, config) =>
                {
                    config.Host(Configuration["RabbitMQURL"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServer"]; //token kontrolü
                opt.Audience = "resource_catalog";
                opt.RequireHttpsMetadata = false;
            });

            services.AddAutoMapper(typeof(Startup));

            #region option pattern creat services

            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(options =>
            {
                return options.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            });

            #endregion

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();

            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter()); //controller üzerine Authorize attributune gerek yok
            });
        }
    }
}
