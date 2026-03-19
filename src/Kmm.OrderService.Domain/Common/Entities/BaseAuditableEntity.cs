namespace Kmm.OrderService.Domain.Common.Entities;

public class BaseAuditableEntity : BaseEntity
{
    public Guid CreatedBy { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Guid? UpdatedBy { get; private set; }

    public DateTime? UpdatedAt { get; private set; }
}
