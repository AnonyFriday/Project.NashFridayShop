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
        Category localCrafts = new CategoryBuilder().WithName("Local Crafts").WithDescription("Handmade traditional crafts").Build();
        Category clothing = new CategoryBuilder().WithName("Clothing & Accessories").WithDescription("Souvenir clothing and wearable items").Build();
        Category food = new CategoryBuilder().WithName("Food & Snacks").WithDescription("Local specialties and snacks").Build();
        Category decor = new CategoryBuilder().WithName("Decor & Gifts").WithDescription("Decorative souvenirs and gift items").Build();
        Category electronics = new CategoryBuilder().WithName("Electronics").WithDescription("Modern gadgets and electronic accessories").Build();
        Category homeLiving = new CategoryBuilder().WithName("Home & Living").WithDescription("Home essentials and lifestyle products").Build();
        Category beauty = new CategoryBuilder().WithName("Beauty & Personal Care").WithDescription("Beauty and self-care products").Build();
        var categories = new List<Category>{
            localCrafts,
            clothing,
            food,
            decor,
            electronics,
            homeLiving,
            beauty
        };
        #endregion

        #region Products
        var products = new List<Product>
        {
            new ProductBuilder().WithCategoryId(localCrafts.Id).WithName("Handwoven Bamboo Basket").WithPrice(15).Build(),
            new ProductBuilder().WithCategoryId(clothing.Id).WithName("Vietnam Souvenir T-Shirt").WithPrice(20).Build(),
            new ProductBuilder().WithCategoryId(food.Id).WithName("Dried Mango Snack").WithPrice(8).Build(),
            new ProductBuilder().WithCategoryId(decor.Id).WithName("Miniature Cyclo Model").WithPrice(25).Build(),
            new ProductBuilder().WithCategoryId(decor.Id).WithName("Lotus Flower Painting").WithPrice(45).Build(),
            new ProductBuilder().WithCategoryId(electronics.Id).WithName("Wireless Bluetooth Earbuds").WithPrice(59).WithQuantity(120).Build(),
            new ProductBuilder().WithCategoryId(electronics.Id).WithName("Portable Power Bank 20000mAh").WithPrice(35).WithQuantity(80).Build(),
            new ProductBuilder().WithCategoryId(electronics.Id).WithName("RGB Mechanical Keyboard").WithPrice(89).WithQuantity(40).Build(),
            new ProductBuilder().WithCategoryId(homeLiving.Id).WithName("Minimalist Desk Lamp").WithPrice(42).WithQuantity(30).Build(),
            new ProductBuilder().WithCategoryId(homeLiving.Id).WithName("Ceramic Coffee Mug Set").WithPrice(18).WithQuantity(65).Build(),
            new ProductBuilder().WithCategoryId(homeLiving.Id).WithName("Wooden Wall Clock").WithPrice(55).WithQuantity(20).Build(),
            new ProductBuilder().WithCategoryId(beauty.Id).WithName("Organic Face Cleanser").WithPrice(22).WithQuantity(90).Build(),
            new ProductBuilder().WithCategoryId(beauty.Id).WithName("Vitamin C Serum").WithPrice(30).WithQuantity(75).Build(),
            new ProductBuilder().WithCategoryId(beauty.Id).WithName("Hydrating Body Lotion").WithPrice(16).WithQuantity(110).Build(),
            new ProductBuilder().WithCategoryId(clothing.Id).WithName("Classic Denim Jacket").WithPrice(70).WithQuantity(25).Build()
        };
        #endregion

        #region ProductRatings
        var productRatings = new List<ProductRating>
        {
            new ProductRatingBuilder().WithProductId(products[0].Id).WithStars(5).WithComment("Very well crafted, feels authentic!").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithStars(4).WithComment("Nice quality but a bit small.").Build(),
            new ProductRatingBuilder().WithProductId(products[1].Id).WithStars(4).WithComment("Comfortable and fits well.").Build(),
            new ProductRatingBuilder().WithProductId(products[1].Id).WithStars(3).WithComment("Design is okay, expected better print.").Build(),
            new ProductRatingBuilder().WithProductId(products[2].Id).WithStars(5).WithComment("Absolutely delicious! Must try.").Build(),
            new ProductRatingBuilder().WithProductId(products[2].Id).WithStars(5).WithComment("Sweet and chewy, loved it.").Build(),
            new ProductRatingBuilder().WithProductId(products[3].Id).WithStars(4).WithComment("Great souvenir piece.").Build(),
            new ProductRatingBuilder().WithProductId(products[4].Id).WithStars(5).WithComment("Beautiful artwork, worth the price.").Build(),
            new ProductRatingBuilder().WithProductId(products[4].Id).WithStars(4).WithComment("Looks good but packaging could improve.").Build(),
            new ProductRatingBuilder().WithProductId(products[5].Id).WithStars(5).WithComment("Sound quality is amazing.").Build(),
            new ProductRatingBuilder().WithProductId(products[5].Id).WithStars(4).WithComment("Battery lasts long enough.").Build(),
            new ProductRatingBuilder().WithProductId(products[6].Id).WithStars(4).WithComment("Charges devices quickly.").Build(),
            new ProductRatingBuilder().WithProductId(products[7].Id).WithStars(5).WithComment("Excellent typing experience.").Build(),
            new ProductRatingBuilder().WithProductId(products[8].Id).WithStars(4).WithComment("Looks modern and elegant.").Build(),
            new ProductRatingBuilder().WithProductId(products[9].Id).WithStars(4).WithComment("Good quality ceramic.").Build(),
            new ProductRatingBuilder().WithProductId(products[10].Id).WithStars(5).WithComment("Beautiful design for living room.").Build(),
            new ProductRatingBuilder().WithProductId(products[11].Id).WithStars(4).WithComment("Feels gentle on skin.").Build(),
            new ProductRatingBuilder().WithProductId(products[12].Id).WithStars(5).WithComment("Skin became brighter after 2 weeks.").Build(),
            new ProductRatingBuilder().WithProductId(products[13].Id).WithStars(4).WithComment("Very moisturizing.").Build(),
            new ProductRatingBuilder().WithProductId(products[14].Id).WithStars(5).WithComment("Fits perfectly and stylish.").Build()
        };
        #endregion

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Products.AddRangeAsync(products);
        await dbContext.ProductRatings.AddRangeAsync(productRatings);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Database seeding completed.");
    }
}
