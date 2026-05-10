using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NashFridayStore.Domain.Entities.Categories;
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
        var customers = new[]
        {
            new { Id = "2c7a9d14-1e5f-4a98-b2e4-3d9f6c0a7e25", Name = "Aria Sterling" },
            new { Id = "3f8c1b7a-4d95-4e2f-a6c1-7b3d9e5f1aaa", Name = "Julian Vane" },
            new { Id = "5d7a2c8f-1b93-4ea6-8fd1-9c2e5b7a1eee", Name = "Elena Rossi" },
            new { Id = "6c3b1a9d-2e84-4f7c-b5a1-9d2e6f3b8c88", Name = "Xavier Thorne" },
            new { Id = "7b6e3d92-8a11-4f5b-90cd-6e3f1a7b2d44", Name = "Seraphina Moon" },
            new { Id = "8a1e5d3c-7f42-4b9a-b3d8-6f1c2e7a9ccc", Name = "Silas Vance" },
            new { Id = "9f1b7f5d-4d83-4c7e-a3b1-8b8f2f7f6a11", Name = "Lyra Belacqua" },
            new { Id = "a5f2d9c8-7b31-4e6a-93fd-8c1b5e2d7f77", Name = "Cyrus Drake" },
            new { Id = "b2d7f4a9-6c18-45eb-91fa-0e7c3b2d8bbb", Name = "Nova Skye" },
            new { Id = "c8d1f6a5-3b7e-4d99-a1c2-5f8e7b6d9a55", Name = "Orion Pax" },
            new { Id = "d7e5a2c1-9b64-41fa-8e3d-4c7b1a9f0d99", Name = "Isolde Thorne" },
            new { Id = "e4c9b1d7-2a56-4f8e-95bc-3a7d1f6e0ddd", Name = "Kaelen Voss" },
            new { Id = "f4d2b8a1-6c93-47ea-9f7c-1a2d5e8b4c30", Name = "Thalia Muse" },
            new { Id = "1e9a4c73-5d28-46bf-8c91-2f7d6a3b0e66", Name = "Vu Thi Rau" }
        };

        var productRatings = new List<ProductRating>();

        // Seed some ratings for the first few products
        // Handwoven Bamboo Basket
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.Parse(customers[0].Id)).WithCustomerName(customers[0].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[0].Id}/avataaars.png").WithStars(5).WithComment("Very well crafted, feels authentic!").Build());
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.Parse(customers[1].Id)).WithCustomerName(customers[1].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[1].Id}/avataaars.png").WithStars(4).WithComment("Nice quality but a bit small.").Build());
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.Parse(customers[2].Id)).WithCustomerName(customers[2].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[2].Id}/avataaars.png").WithStars(5).WithComment("Exceeded my expectations.").Build());

        // Vietnam Souvenir T-Shirt
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[1].Id).WithCustomerId(Guid.Parse(customers[3].Id)).WithCustomerName(customers[3].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[3].Id}/avataaars.png").WithStars(4).WithComment("Comfortable and fits well.").Build());
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[1].Id).WithCustomerId(Guid.Parse(customers[4].Id)).WithCustomerName(customers[4].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[4].Id}/avataaars.png").WithStars(3).WithComment("Design is okay, expected better print.").Build());

        // Dried Mango Snack
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[2].Id).WithCustomerId(Guid.Parse(customers[5].Id)).WithCustomerName(customers[5].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[5].Id}/avataaars.png").WithStars(5).WithComment("Absolutely delicious! Must try.").Build());

        // Miniature Cyclo Model
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[3].Id).WithCustomerId(Guid.Parse(customers[6].Id)).WithCustomerName(customers[6].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[6].Id}/avataaars.png").WithStars(4).WithComment("Great souvenir piece.").Build());

        // Lotus Flower Painting
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[4].Id).WithCustomerId(Guid.Parse(customers[7].Id)).WithCustomerName(customers[7].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[7].Id}/avataaars.png").WithStars(5).WithComment("Beautiful artwork, worth the price.").Build());

        // Wireless Bluetooth Earbuds
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[5].Id).WithCustomerId(Guid.Parse(customers[8].Id)).WithCustomerName(customers[8].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[8].Id}/avataaars.png").WithStars(5).WithComment("Sound quality is amazing.").Build());

        // Portable Power Bank
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[6].Id).WithCustomerId(Guid.Parse(customers[9].Id)).WithCustomerName(customers[9].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[9].Id}/avataaars.png").WithStars(4).WithComment("Charges devices quickly.").Build());

        // RGB Mechanical Keyboard
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[7].Id).WithCustomerId(Guid.Parse(customers[10].Id)).WithCustomerName(customers[10].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[10].Id}/avataaars.png").WithStars(5).WithComment("Excellent typing experience.").Build());

        // Minimalist Desk Lamp
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[8].Id).WithCustomerId(Guid.Parse(customers[11].Id)).WithCustomerName(customers[11].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[11].Id}/avataaars.png").WithStars(4).WithComment("Looks modern and elegant.").Build());

        // Ceramic Coffee Mug Set
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[9].Id).WithCustomerId(Guid.Parse(customers[12].Id)).WithCustomerName(customers[12].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[12].Id}/avataaars.png").WithStars(4).WithComment("Good quality ceramic.").Build());

        // Wooden Wall Clock
        productRatings.Add(new ProductRatingBuilder().WithProductId(products[10].Id).WithCustomerId(Guid.Parse(customers[13].Id)).WithCustomerName(customers[13].Name).WithCustomerAvatarUrl($"https://storage.googleapis.com/nashfridaystore.firebasestorage.app/customers/{customers[13].Id}/avataaars.png").WithStars(5).WithComment("Beautiful design for living room.").Build());
        #endregion

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Products.AddRangeAsync(products);
        await dbContext.ProductRatings.AddRangeAsync(productRatings);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Database seeding completed.");
    }
}
