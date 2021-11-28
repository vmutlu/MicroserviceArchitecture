using DotnetMicroserviceArchitecture.BasketAPI.Services.Abstract;
using DotnetMicroserviceArchitecture.BasketAPI.Services.Concrete;
using DotnetMicroserviceArchitecture.BasketAPI.Settings;
using DotnetMicroserviceArchitecture.Core.Services.Abstract;
using DotnetMicroserviceArchitecture.Core.Services.Concrete;
using DotnetMicroserviceArchitecture.Order.Application.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace DotnetMicroserviceArchitecture.BasketAPI.Extensions
{
    public static class StartupExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            //configure masstransit
            services.AddMassTransit(ms =>
            {
                //add consumer
                ms.AddConsumer<BasketChangeEventConsumer>();

                ms.UsingRabbitMq((context, config) =>
                {
                    config.Host(Configuration["RabbitMQURL"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    config.ReceiveEndpoint("order-service", endp =>
                    {
                        endp.ConfigureConsumer<BasketChangeEventConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); //userId bilgisi taşıyan sub keywordunu mapleme

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServer"]; //token kontrolü
                opt.Audience = "resource_basket";
                opt.RequireHttpsMetadata = false;
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<IBasketService, BasketService>();

            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));

            #region Redis Configuration and Redis Connect Process

            services.AddSingleton<RedisService>(rd =>
            {
                var redisSetting = rd.GetRequiredService<IOptions<RedisSettings>>().Value;

                RedisService redis = new(redisSetting.Host, redisSetting.Port);

                redis.Connect();

                return redis;
            });

            #endregion

            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); //controller üzerine Authorize attributune gerek yok. Authentice olmuş kullanıcı ister
            });
        }
    }
}
