using MediatR;
using OrderSystem.Application.DTOs.Order;

namespace OrderSystem.Application.Orders.Queries.GetOrderById;

public record class GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>
{

}
