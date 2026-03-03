using System;
using AutoMapper;
using OrderSystem.Application.DTOs.Product;
using OrderSystem.Application.Products.Commands.CreateProduct;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductCommand, Product>();
        CreateMap<Product, CreateProductResponseDto>();
        CreateMap<Product, ProductDto>();
    }
}
