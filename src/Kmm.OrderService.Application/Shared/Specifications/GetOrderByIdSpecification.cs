using Kmm.OrderService.Domain.Entities;

namespace Kmm.OrderService.Application.Shared.Specifications;

public sealed class GetOrderByIdSpecification : Specification<Order>
{
    public GetOrderByIdSpecification(Guid id)
    {
        Query.Where(i => i.Id == id)
            .Include(i => i.OrderItems);
    }
}

