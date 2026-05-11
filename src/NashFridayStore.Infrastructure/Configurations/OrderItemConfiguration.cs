using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NashFridayStore.Domain.Entities.Orders;

namespace NashFridayStore.Infrastructure.Configurations;

public sealed class OrderItemConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.Property(x => x.ProductName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CategoryName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.ProductUnitPriceInUsd)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Ignore(x => x.TotalPriceInUsd);

        builder.HasIndex(x => x.CategoryId)
            .HasDatabaseName("ix_order_items_category_id");

        builder.HasIndex(x => x.OrderId)
            .HasDatabaseName("ix_order_items_order_id");

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("ix_order_items_product_id");
    }
}
