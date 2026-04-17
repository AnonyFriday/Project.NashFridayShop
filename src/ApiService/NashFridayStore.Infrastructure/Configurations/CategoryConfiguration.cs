using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NashFridayStore.Domain.Entities;

namespace NashFridayStore.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasIndex(c => c.Name)
            .IsUnique()
            .HasDatabaseName("ix_categories_name");

        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
