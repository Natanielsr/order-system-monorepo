using MediatR;
using OrderSystem.Application.DTOs.Order;

namespace OrderSystem.Application.Orders.Queries.ListOrders;

public record class ListOrdersQuery : IRequest<List<OrderDto>>
{

}
