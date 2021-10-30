using DotnetMicroserviceArchitecture.BasketAPI.Services.Concrete;
using DotnetMicroserviceArchitecture.BasketAPI.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace DotnetMicroserviceArchitecture.BasketAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotnetMicroserviceArchitecture.BasketAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetMicroserviceArchitecture.BasketAPI v1"));
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
