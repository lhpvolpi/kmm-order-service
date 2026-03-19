using Kmm.OrderService.Application.Common.Models;
using Kmm.OrderService.Application.Common.Repositories;
using Kmm.OrderService.Domain.Common.Entities;

namespace Kmm.OrderService.Infrastructure.Common.Repositories;

public class Repository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : BaseEntity
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(TContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public Task<List<TEntity>> GetAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
        => _dbSet.WithSpecification(specification).ToListAsync(cancellationToken);

    public Task<TEntity?> GetFirstAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
        => _dbSet.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);

    public async Task<PaginatedList<TEntity>> ToPaginatedListAsync(
          ISpecification<TEntity> specification,
          CancellationToken cancellationToken = default)
    {
        var totalCount = await SpecificationEvaluator.Default.GetQuery(
            _dbSet.AsQueryable(),
            specification,
            evaluateCriteriaOnly: true)
            .CountAsync(cancellationToken);

        var items = await _dbSet.WithSpecification(specification)
            .ToListAsync(cancellationToken);

        var skip = specification.Skip;
        var take = specification.Take;
        var pageNumber = (skip / take) + 1;
        var pageSize = take;

        return new PaginatedList<TEntity>(items, totalCount, pageNumber, pageSize);
    }

    public void Insert(TEntity entity)
        => _dbSet.Add(entity);

    public void Update(TEntity entity)
        => _context.Entry(entity).State = EntityState.Modified;

    public void Delete(TEntity entity)
        => _dbSet.Remove(entity);
}
