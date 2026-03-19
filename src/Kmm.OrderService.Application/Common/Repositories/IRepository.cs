using Kmm.OrderService.Application.Common.Models;
using Kmm.OrderService.Domain.Common.Entities;

namespace Kmm.OrderService.Application.Common.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetFirstAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default
        );

    Task<List<TEntity>> GetAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default
        );

    public Task<PaginatedList<TEntity>> ToPaginatedListAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default
        );

    void Delete(TEntity entity);

    void Insert(TEntity entity);

    void Update(TEntity entity);
}

