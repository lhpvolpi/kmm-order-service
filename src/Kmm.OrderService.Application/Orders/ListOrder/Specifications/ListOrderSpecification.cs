using Kmm.OrderService.Application.Shared.Specifications;
using Kmm.OrderService.Domain.Entities;

namespace Kmm.OrderService.Application.Orders.ListOrder.Specifications;

public class ListOrderSpecification : PaginatedListSpecification<Order>
{
    public ListOrderSpecification(int pageNumber, int PageSize) : base(pageNumber, PageSize)
    {
        Query.Include(i => i.OrderItems);
    }
}

