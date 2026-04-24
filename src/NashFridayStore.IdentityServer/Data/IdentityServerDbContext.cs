using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Data;

public class IdentityServerDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public IdentityServerDbContext(DbContextOptions options) : base(options)
    {
    }

    protected IdentityServerDbContext()
    {
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("auth");

        builder.ApplyConfigurationsFromAssembly(typeof(IdentityServerDbContext).Assembly);
    }
}
