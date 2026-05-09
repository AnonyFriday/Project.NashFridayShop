using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Extensions;

namespace NashFridayStore.Infrastructure.Configurations;

public sealed class ProductRatingConfiguration : IEntityTypeConfiguration<ProductRating>
{
    public void Configure(EntityTypeBuilder<ProductRating> builder)
    {
        builder.ToTable("product_ratings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CustomerAvatarUrl)
            .HasMaxLength(500);

        builder.Property(x => x.Stars)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(1000);

        builder.HasIndex(x => new { x.ProductId, x.CustomerId })
            .IsUnique()
            .HasDatabaseName("ux_product_ratings_product_customer_id");

        builder.HasIndex(x => x.CustomerId)
            .HasDatabaseName("ix_product_ratings_customer_id");

        builder.HasIndex(x => x.Stars)
            .HasDatabaseName("ix_product_ratings_stars");

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "ck_product_ratings_stars",
                "[Stars] >= 1 AND [Stars] <= 5");
        });

        builder.ConfigureAuditable();
        builder.ConfigureSoftDeletable("product_ratings");
    }
}
