using Kmm.OrderService.Application.Common.Exceptions;
using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Application.Orders.GetOrder.Dtos;
using Kmm.OrderService.Application.Orders.GetOrder.Specifications;

namespace Kmm.OrderService.Application.Orders.GetOrder.Queries;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, GetOrderDto>
{
    private readonly IOrderRepository _orderRepository;

    private readonly IMapper _mapper;

    public GetOrderQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<GetOrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var getOrderSpecification = new GetOrderSpecification(request.Id);
            var order = await _orderRepository.GetFirstAsync(getOrderSpecification, cancellationToken);
            NotFoundException.ThrowIfNull(order, "Order not found");

            return _mapper.Map<GetOrderDto>(order);
        }
        catch
        {
            throw;
        }
    }
}

