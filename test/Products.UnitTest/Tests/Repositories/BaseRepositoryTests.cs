﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Product.Application.Contracts.Repositories;
using Product.Infrastructure.Persistence;
using Product.Infrastructure.Repositories;
using Products.UnitTest.FakeData;
using Xunit;

namespace Products.UnitTest.Tests
{
  /// <summary>
  /// Unit Tests for base Repository<T>
  /// ProductRepository used as an entry point for testing purpose only 
  /// </summary>
  public class BaseRepositoryTests
  {
    private readonly ApplicationContext _context;
    private readonly IProductRepository _repository;

    public BaseRepositoryTests()
    {
      _context = DbInitializer.CreateFakeDatabase();
      _repository = new ProductRepository(_context);
    }

    #region GET Tests

    [Fact]
    public async Task GetAllAsyncReturnsAllProductsAsync()
    {
      var result = await _repository.GetAllAsync();
      result.Count.Should().Be(3);
    }

    [Fact]
    public async void GetByIdAsyncProductExists()
    {
      var result = await _repository.GetByIdAsync(1);
      result.Should().NotBeNull();
      result.Sku.Should().Be("TestSku-1"); 
      result.Description.Should().Be("A description");
      result.Id.Should().Be(1);
    }

    [Fact]
    public async void GetByIdProductNotExists()
    {
      var result = await _repository.GetByIdAsync(5);
      result.Should().BeNull();
    }

    [Fact]
    public async void GetAsyncWithPredicateProductExists()
    {
      var result = await _repository.GetAsync(x => x.Price == 49.99);
      result.Should().NotBeNull();
      result.Count.Should().Be(1);
    }

    [Fact]
    public async void GetAsyncWithPredicateProductNotExists()
    {
      var result = await _repository.GetAsync(x => x.Price == 9876);
      result.Should().NotBeNull();
      result.Count.Should().Be(0);
    }

    [Fact]
    public async void GetFirstOrDefaultAsyncPredicateProductExists()
    {
      var result = await _repository.GetFirstOrDefaultAsync(x => x.Name == "Product A");
      result.Should().NotBeNull();
    }

    [Fact]
    public async void GetFirstOrDefaultAsyncPredicateProductNotExists()
    {
      var result = await _repository.GetFirstOrDefaultAsync(x => x.Name == "xyz132");
      result.Should().BeNull();
    }

    #endregion

    #region ADD Tests

    [Fact]
    public async void AddAsyncProduct()
    {
      var product = await _repository.AddAsync(GetMockProductToAdd());
      await _context.SaveChangesAsync();

      product.Should().NotBeNull();
      product.Id.Should().Be(4);
      product.CreatedBy.Should().Be("sanjyot");
    }

    [Fact]
    public async void AddRangeAsyncProducts()
    {
      await _repository.AddRangeAsync(GetMockProductListToAdd());
      await _context.SaveChangesAsync();

      var result = await _repository.GetAllAsync();
      result.Should().NotBeNull();
      result.Count.Should().Be(5);
    }

    #endregion

    #region UPDATE Tests

    [Fact]
    public async void UpdateAsyncProduct()
    {
      var product = await _repository.GetByIdAsync(1);
      product.Name = "Product X";
      product.Price = 157.2;

      await _repository.UpdateAsync(product);
      await _context.SaveChangesAsync();

      var result = await _repository.GetByIdAsync(1);
      result.Name.Should().Be("Product X");
      result.Price.Should().Be(157.2);
      result.LastModifiedBy.Should().Be("sanjyot");
    }

    #endregion

    #region DELETE Tests

    [Fact]
    public async void DeleteAsyncProductExists()
    {
      var product = await _repository.GetByIdAsync(1);
      await _repository.DeleteAsync(product);
      await _context.SaveChangesAsync();

      var result = await _repository.GetByIdAsync(1);
      result.Should().BeNull();
    }

    [Fact]
    public async void DeleteAsyncProductNotExists()
    {
      var product = await _repository.GetByIdAsync(50);
      product.Should().BeNull();
    }


    #endregion


    #region Private Methods

    private Product.Domain.Entities.Product GetMockProductToAdd() =>
        new()
        {
          Id = 4,
          Sku = "TestSku",
          Description = "D description",
          Name = "Product D",
          Price = 49.99
        };

    private IEnumerable<Product.Domain.Entities.Product> GetMockProductListToAdd() =>
      new List<Product.Domain.Entities.Product>()
      {
          new()
          {
              Id = 5,
              Description = "E description",
              Name = "Product E",
              Price = 49.99
          },
          new()
          {
              Id = 6,
              Description = "F description",
              Name = "Product F",
              Price = 29.99
          }
      }.ToList();

    #endregion
  }
}
