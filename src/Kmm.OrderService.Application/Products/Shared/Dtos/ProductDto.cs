namespace Kmm.OrderService.Application.Products.Shared.Dtos;

public sealed record ProductDto(Guid Id, string Name, decimal UnitPrice, int AvailableQuantity);
