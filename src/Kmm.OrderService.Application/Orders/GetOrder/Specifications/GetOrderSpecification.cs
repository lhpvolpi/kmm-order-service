using Kmm.OrderService.Domain.Entities;

namespace Kmm.OrderService.Application.Orders.GetOrder.Specifications;

public sealed class GetOrderSpecification : Specification<Order>
{
    public GetOrderSpecification(Guid id)
    {
        Query.Where(i => i.Id == id)
            .AsNoTracking()
                .Include(i => i.OrderItems);
    }
}

