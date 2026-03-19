using Kmm.OrderService.Application.Shared.Dtos;

namespace Kmm.OrderService.Application.Orders.ListOrder.Queries;

public sealed record ListOrderQuery : IRequest<PaginatedListDto<OrderDto>>
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
}

