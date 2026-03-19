using Kmm.OrderService.Application.Orders.ConfirmOrder.Dtos;
using Kmm.OrderService.Application.Shared.Commands;

namespace Kmm.OrderService.Application.Orders.ConfirmOrder.Commands;

public sealed record ConfirmOrderCommand(Guid Id) : OrderCommand(Id), IRequest<ConfirmOrderDto>;

