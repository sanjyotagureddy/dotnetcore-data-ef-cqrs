using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Product.Infrastructure.Persistence
{
  public class ApplicationContextSeed
  {
    public static async Task SeedAsync(ApplicationContext applicationContext, ILogger<ApplicationContextSeed> logger)
    {
      if (!applicationContext.Products.Any())
      {
        await applicationContext.Products.AddRangeAsync(GetPreconfiguredOrders());
        await applicationContext.SaveChangesAsync();

        logger.LogInformation("Seed database associated with context {DbContextName}", nameof(ApplicationContext));
      }
    }

    private static IEnumerable<Domain.Entities.Product> GetPreconfiguredOrders()
      => new List<Domain.Entities.Product>
      {
         new()
         {
           Sku = "SonKun",
           Price = 149.99,
           Description = "Test Product description",
           Name = "Test Product"
         }
      };
  }
}