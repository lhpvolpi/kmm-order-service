using Kmm.OrderService.Domain.Common.Entities;
using Kmm.OrderService.Domain.Common.Exceptions;
using Kmm.OrderService.Domain.Enums;

namespace Kmm.OrderService.Domain.Entities;

public sealed class Order : BaseAuditableEntity
{
    private readonly List<OrderItem> _orderItems = new();

    private Order() { } // EF Core

    public Order(Guid customerId, string currency)
    {
        if (customerId == Guid.Empty)
        {
            throw new BusinessValidationException("CustomerId is required.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new BusinessValidationException("Currency is required.");
        }

        CustomerId = customerId;
        Currency = currency;
        Status = EOrderStatus.Placed;
    }

    public Guid CustomerId { get; private set; }

    public EOrderStatus Status { get; private set; }

    public string Currency { get; private set; } = default!;

    public decimal Total { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public void AddItem(Product product, int quantity)
    {
        if (product is null)
        {
            throw new BusinessValidationException("Product is required.");
        }

        if (quantity <= 0)
        {
            throw new BusinessValidationException("Quantity must be greater than zero.");
        }

        if (quantity > product.AvailableQuantity)
        {
            throw new BusinessValidationException("Insufficient stock.");
        }

        _orderItems.Add(new OrderItem(Id, product.Id, product.UnitPrice, quantity));
        RecalculateTotal();
    }

    private void RecalculateTotal()
        => Total = _orderItems.Sum(i => i.UnitPrice * i.Quantity);

    public void Confirm()
    {
        if (Status == EOrderStatus.Confirmed)
        {
            return; // idempotente
        }

        if (Status == EOrderStatus.Canceled)
        {
            throw new BusinessValidationException("Cannot confirm a canceled order.");
        }

        if (Status != EOrderStatus.Placed)
        {
            throw new BusinessValidationException($"Cannot confirm an order with status {Status}.");
        }

        Status = EOrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == EOrderStatus.Canceled)
        {
            return; // idempotente
        }

        if (Status != EOrderStatus.Confirmed)
        {
            throw new BusinessValidationException("Only confirmed orders can be canceled.");
        }

        Status = EOrderStatus.Canceled;
    }
}
