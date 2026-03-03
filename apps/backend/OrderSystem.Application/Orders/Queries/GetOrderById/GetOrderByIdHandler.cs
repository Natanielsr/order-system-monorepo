using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        Order order = (Order)await orderRepository.GetByIdAsync(request.Id);
        OrderDto orderDto = mapper.Map<OrderDto>(order);

        return orderDto;
    }
}
