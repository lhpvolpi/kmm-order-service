using Kmm.OrderService.Application.Orders.CreateOrder.Dtos;

namespace Kmm.OrderService.Application.Orders.CreateOrder.Commands;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    string Currency,
    List<CreateOrderItemCommand> Items
) : IRequest<CreateOrderDto>;

public sealed record CreateOrderItemCommand(Guid ProductId, int Quantity);
