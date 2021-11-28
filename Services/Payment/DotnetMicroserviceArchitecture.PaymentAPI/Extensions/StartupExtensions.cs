using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace DotnetMicroserviceArchitecture.PaymentAPI.Extensions
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

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); //userId bilgisi taşıyan sub keywordunu mapleme

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServer"]; //token kontrolü
                opt.Audience = "resource_payment";
                opt.RequireHttpsMetadata = false;
            });

            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); //controller üzerine Authorize attributune gerek yok. Authentice olmuş kullanıcı ister
            });

        }
    }
}
