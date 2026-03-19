using Kmm.OrderService.Domain.Common.Entities;

namespace Kmm.OrderService.Infrastructure.Common.Configurations;

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(i => i.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();
    }
}

