using Kmm.OrderService.Application.Orders.GetOrder.Dtos;
using Kmm.OrderService.Application.Shared.Commands;

namespace Kmm.OrderService.Application.Orders.GetOrder.Queries;

public sealed record GetOrderQuery(Guid Id) : OrderCommand(Id), IRequest<GetOrderDto>;

