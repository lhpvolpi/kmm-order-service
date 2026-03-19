using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Domain.Entities;
using Kmm.OrderService.Infrastructure.Common.Data;
using Kmm.OrderService.Infrastructure.Common.Repositories;

namespace Kmm.OrderService.Infrastructure.Repositories;

public sealed class ProductRepository : Repository<Product, ApplicationDbContext>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}
