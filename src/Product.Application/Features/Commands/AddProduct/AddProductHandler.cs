using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Product.Application.Contracts.Repositories;

namespace Product.Application.Features.Commands.AddProduct
{
  public class AddProductHandler : IRequestHandler<AddProductCommand, Domain.Entities.Product>
  {
    private readonly ILogger<AddProductHandler> _logger;
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public AddProductHandler(ILogger<AddProductHandler> logger, IProductRepository repository, IMapper mapper)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #region Implementation of IRequestHandler<in AddProductCommand, Product>

    public Task<Domain.Entities.Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
      var product = _mapper.Map<Domain.Entities.Product>(request);
      var newProduct = _repository.AddAsync(product);
      _logger.LogInformation($"Product { newProduct.Id } is successfully created");

      return newProduct;
    }

    #endregion
  }
}