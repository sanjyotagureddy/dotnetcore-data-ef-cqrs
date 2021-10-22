using Product.Domain.Common;

namespace Product.Domain.Entities
{
  public class Product : EntityBase
  {
    public string Name { get; set; }
    public string Sku { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
  }
}