using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Product.Application.Contracts.Repositories;

namespace Product.Application.Features.Queries.GetProduct
{
  public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Domain.Entities.Product>
  {
    private readonly IProductRepository _repository;

    public GetProductQueryHandler(IProductRepository repository)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #region Implementation of IRequestHandler<in GetProductQuery,Product>

    public Task<Domain.Entities.Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
      return _repository.GetByIdAsync(request.Id);
    }

    #endregion
  }
}