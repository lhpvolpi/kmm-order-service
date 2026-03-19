namespace Kmm.OrderService.Application.Shared.Dtos;

public record OrderDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string Currency { get; init; } = default!;
    public decimal Total { get; init; }
    public string Status { get; init; } = default!;
    public IReadOnlyCollection<OrderItemDto> Items { get; init; } = Array.Empty<OrderItemDto>();
}

public sealed record OrderItemDto
{
    public Guid ProductId { get; init; }
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal Subtotal => UnitPrice * Quantity;
}
