using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Product;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Application.Products.Queries.GetAll;

public class GetAllProductsHandler(
    IProductRepository productRepository,
    IMapper mapper
    ) : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync();
        List<ProductDto> productDtos = mapper.Map<List<ProductDto>>(products);
        return productDtos;
    }
}