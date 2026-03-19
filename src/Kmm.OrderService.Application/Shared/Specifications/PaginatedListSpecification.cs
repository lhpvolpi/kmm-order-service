namespace Kmm.OrderService.Application.Shared.Specifications;

public class PaginatedListSpecification<TEntity> : Specification<TEntity> where TEntity : class
{
    public PaginatedListSpecification(int pageNumber = 1, int pageSize = 50)
    {
        Query
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}

