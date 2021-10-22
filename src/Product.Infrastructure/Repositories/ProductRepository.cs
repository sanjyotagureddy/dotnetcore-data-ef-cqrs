using Product.Application.Contracts.Repositories;
using Product.Infrastructure.Persistence;

namespace Product.Infrastructure.Repositories
{
  public class ProductRepository : Repository<Domain.Entities.Product>, IProductRepository
  {
    public ProductRepository(ApplicationContext dbContext)
      : base(dbContext)
    {
    }
  }
}
