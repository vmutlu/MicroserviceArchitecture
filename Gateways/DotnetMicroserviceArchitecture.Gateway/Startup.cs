using DotnetMicroserviceArchitecture.Gateway.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace DotnetMicroserviceArchitecture.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => (Configuration) = (configuration);

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<TokenExchangeHandler>();

            services.AddAuthentication().AddJwtBearer("GatewayScheme", options =>
            {
                options.Authority = Configuration["IdentityServer"]; //token kontrolü
                options.Audience = "resource_gateway";
                options.RequireHttpsMetadata = false;
            });

            services.AddOcelot().AddDelegatingHandler<TokenExchangeHandler>(); //TokenExchangeHandler nezaman devreye girsin ? config dosyasýnda belirtilecek
        }

        async public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            await app.UseOcelot();
        }
    }
}
