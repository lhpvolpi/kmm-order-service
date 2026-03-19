using Kmm.OrderService.Application.Orders.CancelOrder.Dtos;
using Kmm.OrderService.Application.Shared.Commands;

namespace Kmm.OrderService.Application.Orders.CancelOrder.Commands;

public sealed record CancelOrderCommand(Guid Id) : OrderCommand(Id), IRequest<CancelOrderDto>;

