using MediatR;
using OrderSystem.Application.DTOs.Product;

namespace OrderSystem.Application.Products.Queries.GetAll;

public record class GetAllProductsQuery : IRequest<List<ProductDto>>
{
}


