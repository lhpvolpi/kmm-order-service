using Kmm.OrderService.Application.Shared.Dtos;

namespace Kmm.OrderService.Application.Orders.ListOrder.Dtos;

public sealed record ListOrderDto : PaginatedListDto<OrderDto>;
