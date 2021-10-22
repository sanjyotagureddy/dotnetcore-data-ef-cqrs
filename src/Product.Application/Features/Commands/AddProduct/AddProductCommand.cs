using MediatR;

namespace Product.Application.Features.Commands.AddProduct
{
  public class AddProductCommand : Domain.Entities.Product, IRequest<Domain.Entities.Product>
  {

  }
}