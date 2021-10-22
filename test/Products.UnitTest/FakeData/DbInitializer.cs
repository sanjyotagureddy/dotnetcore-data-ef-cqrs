using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Product.Infrastructure.Persistence;

namespace Products.UnitTest.FakeData
{
  public static class DbInitializer
  {
    public static ApplicationContext CreateFakeDatabase()
    {
      var options = new DbContextOptionsBuilder<ApplicationContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

      var context = new ApplicationContext(options);
      SeedDatabase(context);
      return context;
    }

    private static void SeedDatabase(ApplicationContext context)
    {
      context.Products.AddRangeAsync(GetListOfProducts());
      context.SaveChangesAsync();
    }

    private static IEnumerable<Product.Domain.Entities.Product> GetListOfProducts()
    {
      return new List<Product.Domain.Entities.Product>()
      {
          new ()
          {
            Sku = "TestSku-1",
              Description = "A description",
              Name = "Product A",
              Price = 49.99
          },
          new ()
          {
              Description = "B description",
              Name = "Product B",
              Price = 29.99
          },
          new ()
          {
              Description = "C description",
              Name = "Product C",
              Price = 89.99
          }
      };
    }
  }
}
