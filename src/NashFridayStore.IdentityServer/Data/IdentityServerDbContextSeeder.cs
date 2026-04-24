using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.IdentityServer.Commons;
using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Data;

public class IdentityServerDbContextSeeder(
    IdentityServerDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ILogger<IdentityServerDbContext> logger)
{
    public async Task SeedAsync()
    {
        logger.LogInformation("Starting database seeding...");
        await dbContext.Database.MigrateAsync();

        if (await userManager.Users.AnyAsync()
                && await dbContext.Admins.AnyAsync()
                && await dbContext.Customers.AnyAsync())
        {
            logger.LogInformation("Identity DB already seeded. Skipping.");
            return;
        }

        #region Roles 
        string[] roles = [AppCts.Identity.Roles.Admin, AppCts.Identity.Roles.Customer];
        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
        #endregion

        #region Admins
        var admin = new Admin
        {
            Id = Guid.NewGuid(),
            FullName = "Vu Thi Rau",
            Address = "Random Address which you dont know",
            UserName = "admin@nashstore.com",
            Email = "admin@nashstore.com",
            PhoneNumber = "0123456789",
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, AppCts.Identity.Roles.Admin);
        await dbContext.SaveChangesAsync();
        #endregion

        #region Customers
        var customer1 = new Customer
        {
            Id = Guid.NewGuid(),
            FullName = "My meme 1",
            Address = "Random Address which you dont know",
            UserName = "customer1@nashstore.com",
            Email = "customer1@nashstore.com",
            PhoneNumber = "0987654321",
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        await userManager.CreateAsync(customer1, "Customer1@123");
        await userManager.AddToRoleAsync(customer1, AppCts.Identity.Roles.Customer);
        await dbContext.SaveChangesAsync();

        var customer2 = new Customer
        {
            Id = Guid.NewGuid(),
            FullName = "My meme 2",
            Address = "Random Address which you dont know",
            UserName = "customer2@nashstore.com",
            Email = "customer2@nashstore.com",
            PhoneNumber = "0987654321",
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        await userManager.CreateAsync(customer2, "Customer2@123");
        await userManager.AddToRoleAsync(customer2, AppCts.Identity.Roles.Customer);
        await dbContext.SaveChangesAsync();
        #endregion

        logger.LogInformation("Identity database seeding completed.");
    }
}
