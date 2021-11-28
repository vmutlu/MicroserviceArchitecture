using DotnetMicroserviceArchitecture.Gateway.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;

namespace DotnetMicroserviceArchitecture.Gateway.Extensions
{
    public static class StartupExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpClient<TokenExchangeHandler>();

            services.AddAuthentication().AddJwtBearer("GatewayScheme", options =>
            {
                options.Authority = Configuration["IdentityServer"]; //token kontrolü
                options.Audience = "resource_gateway";
                options.RequireHttpsMetadata = false;
            });

            services.AddOcelot().AddDelegatingHandler<TokenExchangeHandler>(); //TokenExchangeHandler nezaman devreye girsin ? config dosyasında belirtilecek
        }
    }
}
