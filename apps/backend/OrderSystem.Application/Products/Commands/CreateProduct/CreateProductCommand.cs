using MediatR;
using OrderSystem.Application.DTOs.Product;

namespace OrderSystem.Application.Products.Commands.CreateProduct;

public record class CreateProductCommand(
    string Name,
    decimal Price,
    int AvailableQuantity,
    Stream FileStream,
    string FileName,
    string ContentType
    ) : IRequest<CreateProductResponseDto>
{

}
