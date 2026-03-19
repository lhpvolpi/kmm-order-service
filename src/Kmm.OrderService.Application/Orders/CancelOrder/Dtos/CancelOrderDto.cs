namespace Kmm.OrderService.Application.Orders.CancelOrder.Dtos;

public sealed record CancelOrderDto
{
    public Guid Id { get; init; }
    public string Status { get; init; } = default!;
}
