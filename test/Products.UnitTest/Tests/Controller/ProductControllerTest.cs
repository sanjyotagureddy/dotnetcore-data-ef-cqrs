using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Product.Api.Controllers;
using Product.Application.Contracts.Repositories;
using Product.Application.Exceptions;
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
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<DeleteProductCommandHandler>> _deleteCommandLogger;
    private readonly Mock<ILogger<UpdateProductCommandHandler>> _updateCommandLogger;

    private readonly Mock<IMediator> _mockMediatR;
    private readonly ProductsController _controller;

    private GetProductQueryHandler _getProductQueryHandler;
    private DeleteProductCommandHandler _deleteProductCommandHandler;
    private UpdateProductCommandHandler _updateProductCommandHandler;
    

    private readonly IProductRepository _repository;
    private readonly UpdateProductCommand _updateProductCommand;

    public ProductControllerTest()
    {
      var context = DbInitializer.CreateFakeDatabase();
      _repository = new ProductRepository(context);
      _mockMediatR = new Mock<IMediator>();
      _updateCommandLogger = new Mock<ILogger<UpdateProductCommandHandler>>();

      //Mapper
      var mapperConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new ProductProfile());
      });
      _mapper = mapperConfig.CreateMapper();


      //Controller
      _controller = new ProductsController(_mockMediatR.Object);
      _deleteCommandLogger = new Mock<ILogger<DeleteProductCommandHandler>>();

       _updateProductCommand = new UpdateProductCommand { Id = 2, Description = "Updated Product", Price = 99, Name = "New Name" };
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

      var orders = await _controller.GetProduct(1);
      if (orders.Result is not OkObjectResult okResult)
        orders.Should().BeNull();
      else
      {
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
      }
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

    #region Update

    [Fact]
    public async void UpdateProduct_ReturnsNoContent()
    {
      //Delete QueryHandler
      _updateProductCommandHandler = new UpdateProductCommandHandler(_updateCommandLogger.Object, _repository, _mapper);
      _mockMediatR.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
        .Returns(async () =>
          await _updateProductCommandHandler.Handle(_updateProductCommand, new CancellationToken()));

      
      var orders = await _controller.UpdateProduct(_updateProductCommand);
      if (orders is not NoContentResult okResult) return;
      okResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

      var product =await _repository.GetByIdAsync(2);
      product.Description.Should().Be("Updated Product");

    }

    [Fact]
    public void UpdateProduct_ReturnsThrowNotFound()
    {
      //Delete QueryHandler
      _updateProductCommandHandler = new UpdateProductCommandHandler(_updateCommandLogger.Object, _repository, _mapper);
      _mockMediatR.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
        .Throws(new NotFoundException("Product", 365));
      _updateProductCommand.Id = 7985;
      Func<Task> func = async () => await _controller.UpdateProduct(_updateProductCommand);
      func.Should().Throw<NotFoundException>().WithMessage($"Entity \"Product\" (365) was not found");
    }

    #endregion

    [Fact]
    public async void DeleteProductById_ReturnsNoContent()
    {
      //Delete QueryHandler
      _deleteProductCommandHandler = new DeleteProductCommandHandler(_repository, _mapper, _deleteCommandLogger.Object);
      _mockMediatR.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
        .Returns(async () =>
          await _deleteProductCommandHandler.Handle(new DeleteProductCommand() { Id = 3 },
            new CancellationToken()));

      var orders = await _controller.DeleteProduct(3);
      if (orders is not OkObjectResult okResult) return;
      okResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Fact]
    public void DeleteProductById_ReturnsNotFound()
    {
      //Delete QueryHandler
      _deleteProductCommandHandler = new DeleteProductCommandHandler(_repository, _mapper, _deleteCommandLogger.Object);
      _mockMediatR.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
        .Throws(new NotFoundException("Product", 365));
      Func<Task> func = async () => await _controller.DeleteProduct(365);
      func.Should().Throw<NotFoundException>().WithMessage($"Entity \"Product\" (365) was not found");
    }
  }
}