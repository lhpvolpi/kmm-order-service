using Kmm.OrderService.Domain.Entities;

namespace Kmm.OrderService.Application.Products.ListProduct.Specifications;

public sealed class ListProductSpecification : Specification<Product>
{
    public ListProductSpecification(string? search = null)
    {
        var term = string.IsNullOrWhiteSpace(search) ? null : search.Trim().ToLowerInvariant();
        Query.Where(i => term == null || i.Name.ToLower()!.Contains(term));
    }
}

