using System.Net;
using System.Threading;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Product.Api.Controllers;
using Product.Application.Contracts.Repositories;
using Product.Application.Features.Commands.DeleteProduct;
using Product.Application.Features.Commands.UpdateProduct;
using Product.Application.Features.Queries.GetProduct;
using Product.Application.Mappings;
using Product.Infrastructure.Repositories;
using Products.UnitTest.FakeData;
using Xunit;

namespace Products.UnitTest.Tests.Controller
{
  public class ProductControllerTest
  {
    private Mock<IMediator> _mediator;
    private IMapper _mapper;
    private Mock<ILogger<DeleteProductCommandHandler>> _deleteCommandLogger;
    private Mock<ILogger<UpdateProductCommandHandler>> _updateCommandLogger;

    private readonly Mock<IMediator> _mockMediatR;
    private readonly ProductsController _controller;

    private GetProductQueryHandler _getProductQueryHandler;
    private DeleteProductCommandHandler _deleteProductCommandHandler;
    private UpdateProductCommandHandler _updateProductCommandHandler;

    private readonly Product.Domain.Entities.Product _product;

    private readonly IProductRepository _repository;
    public ProductControllerTest()
    {
      _mediator = new Mock<IMediator>();
      var context = DbInitializer.CreateFakeDatabase();
      _repository = new ProductRepository(context);
      _mockMediatR = new Mock<IMediator>();

      //Mapper
      var mapperConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new ProductProfile());
      });
      _mapper = mapperConfig.CreateMapper();


      //Controller
      _controller = new ProductsController(_mockMediatR.Object);
      _deleteCommandLogger = new Mock<ILogger<DeleteProductCommandHandler>>();
    }

    [Fact]
    public async void GetProductById_Returns200OK()
    {
      //GET QueryHandler
      _getProductQueryHandler = new GetProductQueryHandler(_repository);
      _mockMediatR.Setup(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
        .Returns(async () =>
          await _getProductQueryHandler.Handle(new GetProductQuery(1),
            new CancellationToken()));

      var orders =await _controller.GetProduct(1);
      if (orders.Result is not OkObjectResult okResult) return;
      okResult.Should().NotBeNull();
      okResult.StatusCode.Should().Be((int) HttpStatusCode.OK);

    }

    [Fact]
    public async void GetProductById_ReturnsNull()
    {
      //GET QueryHandler
      _getProductQueryHandler = new GetProductQueryHandler(_repository);
      _mockMediatR.Setup(m => m.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
        .Returns(async () =>
          await _getProductQueryHandler.Handle(new GetProductQuery(61),
            new CancellationToken()));

      var orders = await _controller.GetProduct(1);
      if (orders.Result is not ObjectResult okResult) return;
      okResult.Value.Should().BeNull();

    }

  }
}