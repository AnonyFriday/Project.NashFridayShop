using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Builders;

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

        #region Categories
        Category localCrafts = new CategoryBuilder()
           .WithName("Local Crafts")
           .WithDescription("Handmade traditional crafts")
           .Build();

        Category clothing = new CategoryBuilder()
            .WithName("Clothing & Accessories")
            .WithDescription("Souvenir clothing and wearable items")
            .Build();

        Category food = new CategoryBuilder()
            .WithName("Food & Snacks")
            .WithDescription("Local specialties and snacks")
            .Build();

        Category decor = new CategoryBuilder()
            .WithName("Decor & Gifts")
            .WithDescription("Decorative souvenirs and gift items")
            .Build();

        var cate = new List<Category>{
            localCrafts,
            clothing,
            food,
            decor
        };
        #endregion

        #region Products
        var products = new List<Product>
        {
            new ProductBuilder().WithCategoryId(localCrafts.Id).WithName("Handwoven Bamboo Basket").WithPrice(15).Build(),
            new ProductBuilder().WithCategoryId(clothing.Id).WithName("Vietnam Souvenir T-Shirt").WithPrice(20).Build(),
            new ProductBuilder().WithCategoryId(food.Id).WithName("Dried Mango Snack").WithPrice(8).Build(),
            new ProductBuilder().WithCategoryId(decor.Id).WithName("Miniature Cyclo Model").WithPrice(25).Build(),
            new ProductBuilder().WithCategoryId(decor.Id).WithName("Lotus Flower Painting").WithPrice(45).Build()
        };
        #endregion

        await dbContext.Categories.AddRangeAsync(cate);
        await dbContext.Products.AddRangeAsync(products);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Database seeding completed.");
    }
}
