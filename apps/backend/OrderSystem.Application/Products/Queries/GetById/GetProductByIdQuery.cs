using MediatR;
using OrderSystem.Application.DTOs.Product;

namespace OrderSystem.Application.Products.Queries.GetById;

public record class GetProductByIdQuery(Guid id) : IRequest<ProductDto>
{

}
