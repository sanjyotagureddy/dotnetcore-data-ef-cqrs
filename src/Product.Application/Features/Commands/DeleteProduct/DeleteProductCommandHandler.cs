using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Product.Application.Contracts.Repositories;
using Product.Application.Exceptions;

namespace Product.Application.Features.Commands.DeleteProduct
{
  public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
  {
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(IProductRepository repository, IMapper mapper,
      ILogger<DeleteProductCommandHandler> logger)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Implementation of IRequestHandler<in DeleteProductCommand,Unit>

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
      var product = await _repository.GetByIdAsync(request.Id);
      if (product is null)
        throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);

      await _repository.DeleteAsync(product);
      _logger.LogInformation($"Product {product.Id} is successfully deleted");

      return Unit.Value;
    }

    #endregion
  }
}