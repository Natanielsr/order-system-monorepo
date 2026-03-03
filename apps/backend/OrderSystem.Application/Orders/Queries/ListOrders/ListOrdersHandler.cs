using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Application.Orders.Queries.ListOrders;

public class ListOrdersHandler : IRequestHandler<ListOrdersQuery, List<OrderDto>>
{
    private readonly IOrderRepository _repository; // Ou seu DbContext diretamente
    private readonly IMapper _mapper;

    public ListOrdersHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetAllAsync();

        // Mapeamento para DTO (pode usar AutoMapper ou manual)
        var ordersDto = _mapper.Map<List<OrderDto>>(orders);

        return ordersDto;
    }
}
