using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Commands.AddProduct;
using Product.Application.Features.Commands.DeleteProduct;
using Product.Application.Features.Commands.UpdateProduct;
using Product.Application.Features.Queries.GetProduct;

namespace Product.Api.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
      _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("{id}", Name = "GetProduct")]
    [ProducesResponseType(typeof(Domain.Entities.Product), StatusCodes.Status200OK)]
    public async Task<ActionResult<Domain.Entities.Product>> GetProduct(int id)
    {
      var query = new GetProductQuery(id);
      var orders = await _mediator.Send(query);
      return Ok(orders);
    }

    [HttpPost(Name = "AddProduct")]
    [ProducesResponseType(typeof(Domain.Entities.Product), StatusCodes.Status201Created)]
    public async Task<ActionResult<Domain.Entities.Product>> AddProduct([FromBody] AddProductCommand command)
    {
      var result = await _mediator.Send(command);
      return Ok(result);
    }
    [HttpPut(Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
    {
      await _mediator.Send(command);
      return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteProduct(int id)
    {
      var command = new DeleteProductCommand() { Id = id };
      await _mediator.Send(command);
      return NoContent();
    }
  }
}
