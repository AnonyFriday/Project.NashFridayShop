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

        if (await dbContext.Categories.AnyAsync()
            && await dbContext.Products.AnyAsync()
            && await dbContext.ProductRatings.AnyAsync())
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

        var categories = new List<Category>{
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

        #region ProductRatings
        var productRatings = new List<ProductRating>
        {
            // Bamboo Basket
            new ProductRatingBuilder()
                .WithProductId(products[0].Id)
                .WithStars(9)
                .WithComment("Very well crafted, feels authentic!")
                .Build(),

            new ProductRatingBuilder()
                .WithProductId(products[0].Id)
                .WithStars(8)
                .WithComment("Nice quality but a bit small.")
                .Build(),

            // T-Shirt
            new ProductRatingBuilder()
                .WithProductId(products[1].Id)
                .WithStars(7)
                .WithComment("Comfortable and fits well.")
                .Build(),

            new ProductRatingBuilder()
                .WithProductId(products[1].Id)
                .WithStars(6)
                .WithComment("Design is okay, expected better print.")
                .Build(),

            // Dried Mango
            new ProductRatingBuilder()
                .WithProductId(products[2].Id)
                .WithStars(10)
                .WithComment("Absolutely delicious! Must try.")
                .Build(),

            new ProductRatingBuilder()
                .WithProductId(products[2].Id)
                .WithStars(9)
                .WithComment("Sweet and chewy, loved it.")
                .Build(),

            // Cyclo Model
            new ProductRatingBuilder()
                .WithProductId(products[3].Id)
                .WithStars(8)
                .WithComment("Great souvenir piece.")
                .Build(),

            // Lotus Painting
            new ProductRatingBuilder()
                .WithProductId(products[4].Id)
                .WithStars(9)
                .WithComment("Beautiful artwork, worth the price.")
                .Build(),

            new ProductRatingBuilder()
                .WithProductId(products[4].Id)
                .WithStars(7)
                .WithComment("Looks good but packaging could improve.")
                .Build()
        };
        #endregion

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Products.AddRangeAsync(products);
        await dbContext.ProductRatings.AddRangeAsync(productRatings);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Database seeding completed.");
    }
}
