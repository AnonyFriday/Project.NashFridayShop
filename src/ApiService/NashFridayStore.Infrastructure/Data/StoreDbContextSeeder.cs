using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.Infrastructure.Data;

public class StoreDbContextSeeder(StoreDbContext dbContext, ILogger<StoreDbContextSeeder> logger)
{
    public async Task SeedAsync()
    {
        logger.LogInformation("Starting database seeding...");
        await dbContext.Database.MigrateAsync();

        if (await dbContext.Categories.AnyAsync())
        {
            logger.LogInformation("Database already seeded. Skipping seeding.");
            return;
        }

        DateTime now = DateTime.UtcNow;

        var categories = new List<Category>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Local Crafts",
                Description = "Handmade traditional crafts",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Clothing & Accessories",
                Description = "Souvenir clothing and wearable items",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Food & Snacks",
                Description = "Local specialties and snacks",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Decor & Gifts",
                Description = "Decorative souvenirs and gift items",
            }
        };

        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CategoryId = categories[0].Id,
                Name = "Handwoven Bamboo Basket",
                Description = "Traditional handcrafted bamboo basket from local artisans.",
                PriceUsd = 15,
                ImageUrl = "https://picsum.photos/300?random=1",
                Quantity = 40,
                Status = ProductStatus.InStock,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                IsDeleted = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                CategoryId = categories[1].Id,
                Name = "Vietnam Souvenir T-Shirt",
                Description = "Cotton T-shirt with iconic Vietnam print.",
                PriceUsd = 20,
                ImageUrl = "https://picsum.photos/300?random=2",
                Quantity = 100,
                Status = ProductStatus.InStock,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                IsDeleted = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                CategoryId = categories[2].Id,
                Name = "Dried Mango Snack",
                Description = "Sweet and chewy dried mango from local farms.",
                PriceUsd = 8,
                ImageUrl = "https://picsum.photos/300?random=3",
                Quantity = 60,
                Status = ProductStatus.InStock,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                IsDeleted = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                CategoryId = categories[3].Id,
                Name = "Miniature Cyclo Model",
                Description = "Decorative miniature cyclo, a symbol of Vietnam.",
                PriceUsd = 25,
                ImageUrl = "https://picsum.photos/300?random=4",
                Quantity = 25,
                Status = ProductStatus.InStock,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                IsDeleted = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                CategoryId = categories[3].Id,
                Name = "Lotus Flower Painting",
                Description = "Hand-painted lotus artwork representing purity.",
                PriceUsd = 45,
                ImageUrl = "https://picsum.photos/300?random=5",
                Quantity = 10,
                Status = ProductStatus.InStock,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                IsDeleted = false
            }
        };

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Products.AddRangeAsync(products);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Database seeding completed.");
    }
}
