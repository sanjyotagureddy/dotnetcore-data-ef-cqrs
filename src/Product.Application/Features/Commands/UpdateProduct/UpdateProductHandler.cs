using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Product.Application.Contracts.Repositories;
using Product.Application.Exceptions;

namespace Product.Application.Features.Commands.UpdateProduct
{
  public class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
  {
    private readonly ILogger<UpdateProductHandler> _logger;
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProductHandler(ILogger<UpdateProductHandler> logger, IProductRepository repository, IMapper mapper)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #region Implementation of IRequestHandler<in AddProductCommand, Product>

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
      var product = await _repository.GetByIdAsync(request.Id);
      if (product is null)
        throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
      var productToUpdate = _mapper.Map(request, product);
      await _repository.UpdateAsync(productToUpdate);

      _logger.LogInformation($"Product { productToUpdate.Id } is successfully updated");

      return Unit.Value;
    }

    #endregion
  }
}