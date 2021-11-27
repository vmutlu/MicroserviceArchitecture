using DotnetMicroserviceArchitecture.Core.Services.Abstract;
using DotnetMicroserviceArchitecture.Core.Services.Concrete;
using DotnetMicroserviceArchitecture.Order.Application.Consumers;
using DotnetMicroserviceArchitecture.Order.Infrastructure.Context;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace DotnetMicroserviceArchitecture.OrderAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => (Configuration) = (configuration);

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //configure masstransit
            services.AddMassTransit(ms =>
            {
                //add consumer
                ms.AddConsumer<CreateOrderMessageCommandConsumer>();
                ms.AddConsumer<CatalogNameChangeEventConsumer>();

                ms.UsingRabbitMq((context, config) =>
                {
                    config.Host(Configuration["RabbitMQURL"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    config.ReceiveEndpoint("order-service", endp =>
                     {
                         endp.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
                     });

                    config.ReceiveEndpoint("order-service-change", endp =>
                    {
                        endp.ConfigureConsumer<CatalogNameChangeEventConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); //userId bilgisi ta��yan sub keywordunu mapleme

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServer"]; //token kontrol�
                opt.Audience = "resource_order";
                opt.RequireHttpsMetadata = false;
            });

            services.AddMediatR(typeof(DotnetMicroserviceArchitecture.Order.Application.Handlers.GetOrderByUserIdQueryHandler).Assembly);

            services.AddHttpContextAccessor();
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddDbContext<OrderContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), configure =>
            {
                configure.MigrationsAssembly("DotnetMicroserviceArchitecture.Order.Infrastructure");
            }));

            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); //controller �zerine Authorize attributune gerek yok. Authentice olmu� kullan�c� ister
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotnetMicroserviceArchitecture.OrderAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetMicroserviceArchitecture.OrderAPI v1"));
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
