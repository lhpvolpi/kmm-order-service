using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Application.Orders.ListOrder.Specifications;
using Kmm.OrderService.Application.Shared.Dtos;

namespace Kmm.OrderService.Application.Orders.ListOrder.Queries;

public class ListOrderQueryHandler : IRequestHandler<ListOrderQuery, PaginatedListDto<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    private readonly IMapper _mapper;

    public ListOrderQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedListDto<OrderDto>> Handle(ListOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var specification = new ListOrderSpecification(request.PageNumber, request.PageSize);
            var paginatedList = await _orderRepository.ToPaginatedListAsync(specification, cancellationToken);

            return _mapper.Map<PaginatedListDto<OrderDto>>(paginatedList);
        }
        catch
        {
            throw;
        }
    }
}

