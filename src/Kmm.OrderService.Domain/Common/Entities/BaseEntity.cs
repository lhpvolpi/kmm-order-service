namespace Kmm.OrderService.Domain.Common.Entities;

public class BaseEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}
