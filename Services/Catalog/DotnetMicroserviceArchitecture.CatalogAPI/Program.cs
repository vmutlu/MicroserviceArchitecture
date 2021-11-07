using DotnetMicroserviceArchitecture.CatalogAPI.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace DotnetMicroserviceArchitecture.CatalogAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var catalogService = serviceProvider.GetRequiredService<ICategoryService>();

                if (!catalogService.GetAllAsync().Result.Data.Any())
                {
                    catalogService.AddAsync(new Dtos.CategoryDTO()
                    {
                        Name = "Example Category - 1"
                    }).Wait();

                    catalogService.AddAsync(new Dtos.CategoryDTO()
                    {
                        Name = "Example Category - 2"
                    }).Wait();

                    catalogService.AddAsync(new Dtos.CategoryDTO()
                    {
                        Name = "Example Category - 3"
                    }).Wait();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
