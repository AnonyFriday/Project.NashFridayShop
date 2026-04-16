using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.Infrastructure.Data;

public class StoreDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);
    }
}
