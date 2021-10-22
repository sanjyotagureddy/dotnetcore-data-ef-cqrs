using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Product.Infrastructure.Extensions;
using Product.Infrastructure.Persistence;

namespace Product.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();
      host.MigrateDatabase<ApplicationContext>((context, service) =>
       {
         var logger = service.GetService<ILogger<ApplicationContextSeed>>();
         ApplicationContextSeed.SeedAsync(context, logger).Wait();
       });
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
