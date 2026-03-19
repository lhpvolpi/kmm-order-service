using Kmm.OrderService.Domain.Entities;

namespace Kmm.OrderService.Infrastructure.Common.Configurations;

public sealed class ProductConfiguration : BaseAuditableEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("products");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.UnitPrice)
            .HasColumnName("unit_price")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(i => i.AvailableQuantity)
            .HasColumnName("available_quantity")
            .IsRequired();

        builder.Property(i => i.Xmin)
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsRowVersion();

        builder.HasIndex(i => i.Name);
    }
}
