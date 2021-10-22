using MediatR;

namespace Product.Application.Features.Commands.UpdateProduct
{
  public class UpdateProductCommand : IRequest
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
  }
}