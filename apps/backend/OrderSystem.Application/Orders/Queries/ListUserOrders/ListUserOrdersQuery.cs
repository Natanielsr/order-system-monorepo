using MediatR;
using OrderSystem.Application.DTOs.Order;

namespace OrderSystem.Application.Orders.Queries.ListUserOrders;

public record class ListUserOrdersQuery(Guid UserId, int page, int pageSize) : IRequest<List<OrderDto>>
{

}
