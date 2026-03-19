namespace Kmm.OrderService.Application.Orders.ConfirmOrder.Dtos;

public sealed record ConfirmOrderDto
{
    public Guid Id { get; init; }
    public string Status { get; init; } = default!;
}
