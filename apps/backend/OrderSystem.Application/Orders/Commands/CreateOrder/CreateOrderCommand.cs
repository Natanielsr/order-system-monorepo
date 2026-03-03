using MediatR;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Orders.Commands.CreateOrder;

public record class CreateOrderCommand(
    List<CreateOrderItemDto> OrderItems,
    Guid UserId,
    PaymentMethod PaymentMethod,
    Guid AddressId
    ) : IRequest<CreateOrderResponseDto>
{

}
