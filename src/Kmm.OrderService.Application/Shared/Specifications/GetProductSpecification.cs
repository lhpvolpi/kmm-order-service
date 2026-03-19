using Kmm.OrderService.Domain.Entities;

namespace Kmm.OrderService.Application.Shared.Specifications;

public sealed class GetProductSpecification : Specification<Product>
{
    public GetProductSpecification(List<Guid> ids)
    {
        Query.Where(i => ids.Contains(i.Id));
    }

    public GetProductSpecification(Guid id)
    {
        Query.Where(i => i.Id == id);
    }
}

