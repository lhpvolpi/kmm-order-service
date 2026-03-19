using Kmm.OrderService.Domain.Common.Entities;
using Kmm.OrderService.Domain.Common.Exceptions;

namespace Kmm.OrderService.Domain.Entities;

public sealed class Product : BaseAuditableEntity
{
    private Product() { } // EF Core

    public Product(string name, decimal unitPrice, int availableQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessValidationException("Product name is required.");
        }

        if (unitPrice <= 0)
        {
            throw new BusinessValidationException("UnitPrice must be greater than zero.");
        }

        if (availableQuantity < 0)
        {
            throw new BusinessValidationException("AvailableQuantity cannot be negative.");
        }

        Name = name;
        UnitPrice = unitPrice;
        AvailableQuantity = availableQuantity;
    }

    public string Name { get; private set; } = default!;
    public decimal UnitPrice { get; private set; }
    public int AvailableQuantity { get; private set; }
    public uint Xmin { get; private set; }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new BusinessValidationException("Quantity must be greater than zero.");
        }

        if (AvailableQuantity < quantity)
        {
            throw new BusinessValidationException("Insufficient stock.");
        }

        AvailableQuantity -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new BusinessValidationException("Quantity must be greater than zero.");
        }

        AvailableQuantity += quantity;
    }
}
