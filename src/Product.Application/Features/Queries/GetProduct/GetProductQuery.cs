using MediatR;

namespace Product.Application.Features.Queries.GetProduct
{
  public class GetProductQuery : IRequest<Domain.Entities.Product>
  {
    public int Id { get; set; }

    public GetProductQuery(int id) => Id = id;
  }
}