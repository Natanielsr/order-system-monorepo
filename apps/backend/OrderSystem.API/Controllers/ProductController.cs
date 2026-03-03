using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.Products.Commands.CreateProduct;
using OrderSystem.Application.Products.Queries.GetAll;
using OrderSystem.Application.Products.Queries.GetById;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.API.Controllers
{
    public record ProductInput
    {
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
        public required IFormFile Image { get; set; } // O Swagger ama o IFormFile
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(
        IMediator mediator
        ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductInput productInput)
        {
            using var stream = productInput.Image.OpenReadStream();

            CreateProductCommand createProductCommand = new(
                productInput.Name,
                productInput.Price,
                productInput.AvailableQuantity,
                stream,
                productInput.Image.FileName,
                productInput.Image.ContentType
            );

            var response = await mediator.Send(createProductCommand);

            return CreatedAtRoute("GetById", new { id = response.Id }, response);
        }

        [HttpGet("{id:guid}", Name = "GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await mediator.Send(new GetProductByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }
    }
}
