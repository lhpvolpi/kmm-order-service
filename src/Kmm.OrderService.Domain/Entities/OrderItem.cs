using Kmm.OrderService.Domain.Common.Entities;
using Kmm.OrderService.Domain.Common.Exceptions;

namespace Kmm.OrderService.Domain.Entities;

public sealed class OrderItem : BaseAuditableEntity
{
    private OrderItem() { } // EF Core

    public OrderItem(Guid orderId, Guid productId, decimal unitPrice, int quantity)
    {
        if (orderId == Guid.Empty)
        {
            throw new BusinessValidationException("OrderId is required.");
        }

        if (productId == Guid.Empty)
        {
            throw new BusinessValidationException("ProductId is required.");
        }

        if (unitPrice <= 0)
        {
            throw new BusinessValidationException("UnitPrice must be greater than zero.");
        }

        if (quantity <= 0)
        {
            throw new BusinessValidationException("Quantity must be greater than zero.");
        }

        OrderId = orderId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public Order Order { get; private set; } = default!;
    public Product Product { get; private set; } = default!;
}

