using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Application.Orders.Queries.ListUserOrders;

public class ListUserOrdersHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<ListUserOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(ListUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetAllUserOrdersAsync(request.UserId, request.page, request.pageSize);
        var ordersDto = mapper.Map<List<OrderDto>>(orders);

        return ordersDto;
    }
}
