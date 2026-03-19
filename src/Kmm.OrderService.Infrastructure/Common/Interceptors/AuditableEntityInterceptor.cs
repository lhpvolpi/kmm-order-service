using Kmm.OrderService.Application.Common.Entities;
using Kmm.OrderService.Domain.Common.Entities;

namespace Kmm.OrderService.Infrastructure.Common.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IUser _user;

    public AuditableEntityInterceptor(IUser user) => _user = user;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Apply(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Apply(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void Apply(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var utcNow = DateTime.UtcNow;
        var customerId = _user.CustomerId;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(e => e.CreatedBy).CurrentValue = customerId;
                entry.Property(e => e.CreatedAt).CurrentValue = utcNow;
            }
            else if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Property(e => e.UpdatedBy).CurrentValue = customerId;
                entry.Property(e => e.UpdatedAt).CurrentValue = utcNow;
            }
        }
    }
}

internal static class EntityEntryExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
        => entry.References.Any(r => r.TargetEntry is not null
                                    && r.TargetEntry.Metadata.IsOwned()
                                    && (r.TargetEntry.State == EntityState.Added
                                        || r.TargetEntry.State == EntityState.Modified));
}
