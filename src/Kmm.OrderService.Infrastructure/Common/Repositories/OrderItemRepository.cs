using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Domain.Entities;
using Kmm.OrderService.Infrastructure.Common.Data;
using Kmm.OrderService.Infrastructure.Common.Repositories;

namespace Kmm.OrderService.Infrastructure.Repositories;

public sealed class OrderItemRepository : Repository<OrderItem, ApplicationDbContext>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}
