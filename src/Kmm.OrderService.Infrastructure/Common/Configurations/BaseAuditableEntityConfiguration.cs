using Kmm.OrderService.Domain.Common.Entities;

namespace Kmm.OrderService.Infrastructure.Common.Configurations;

public class BaseAuditableEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity>
    where TEntity : BaseAuditableEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(i => i.CreatedBy)
            .HasColumnName("created_by")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(i => i.UpdatedBy)
            .HasColumnName("updated_by")
            .HasColumnType("uuid");

        builder.Property(i => i.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamptz");
    }
}
