using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Product;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Application.Products.Queries.GetById;

public class GetProductByIdHandler(
    IProductRepository productRepository,
    IMapper mapper
    ) : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.id);
        var productDto = mapper.Map<ProductDto>(product);

        return productDto;
    }
}
