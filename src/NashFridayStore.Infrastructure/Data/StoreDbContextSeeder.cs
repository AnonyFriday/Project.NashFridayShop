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
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("John Doe").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=john").WithStars(5).WithComment("Very well crafted, feels authentic!").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Jane Smith").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=jane").WithStars(4).WithComment("Nice quality but a bit small.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Alice Johnson").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=alice").WithStars(5).WithComment("Exceeded my expectations.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Bob Brown").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=bob").WithStars(4).WithComment("Beautiful texture and very sturdy.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Charlie Davis").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=charlie").WithStars(5).WithComment("Perfect for my home decor.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Diana Prince").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=diana").WithStars(3).WithComment("Good, but the color is slightly different from the photo.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Evan Wright").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=evan").WithStars(5).WithComment("Highly recommend this!").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Fiona Green").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=fiona").WithStars(4).WithComment("Great value for money.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("George Miller").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=george").WithStars(5).WithComment("Love the craftsmanship.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Hannah Baker").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=hannah").WithStars(4).WithComment("Solid build and fast shipping.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Ian Somerhalder").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=ian").WithStars(5).WithComment("A piece of art!").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Julia Roberts").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=julia").WithStars(5).WithComment("Will definitely buy again.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Kevin Hart").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=kevin").WithStars(4).WithComment("Very happy with this purchase.").Build(),
            new ProductRatingBuilder().WithProductId(products[0].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Laura Croft").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=laura").WithStars(5).WithComment("Amazing quality bamboo!").Build(),
            new ProductRatingBuilder().WithProductId(products[1].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Michael Scott").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=michael").WithStars(4).WithComment("Comfortable and fits well.").Build(),
            new ProductRatingBuilder().WithProductId(products[1].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Nancy Wheeler").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=nancy").WithStars(3).WithComment("Design is okay, expected better print.").Build(),
            new ProductRatingBuilder().WithProductId(products[2].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Oscar Isaac").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=oscar").WithStars(5).WithComment("Absolutely delicious! Must try.").Build(),
            new ProductRatingBuilder().WithProductId(products[2].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Pam Beesly").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=pam").WithStars(5).WithComment("Sweet and chewy, loved it.").Build(),
            new ProductRatingBuilder().WithProductId(products[3].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Quentin Tarantino").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=quentin").WithStars(4).WithComment("Great souvenir piece.").Build(),
            new ProductRatingBuilder().WithProductId(products[4].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Rachel Green").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=rachel").WithStars(5).WithComment("Beautiful artwork, worth the price.").Build(),
            new ProductRatingBuilder().WithProductId(products[4].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Steve Harrington").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=steve").WithStars(4).WithComment("Looks good but packaging could improve.").Build(),
            new ProductRatingBuilder().WithProductId(products[5].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Tina Fey").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=tina").WithStars(5).WithComment("Sound quality is amazing.").Build(),
            new ProductRatingBuilder().WithProductId(products[5].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Ursula Buffay").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=ursula").WithStars(4).WithComment("Battery lasts long enough.").Build(),
            new ProductRatingBuilder().WithProductId(products[6].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Victor Stone").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=victor").WithStars(4).WithComment("Charges devices quickly.").Build(),
            new ProductRatingBuilder().WithProductId(products[7].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Wanda Maximoff").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=wanda").WithStars(5).WithComment("Excellent typing experience.").Build(),
            new ProductRatingBuilder().WithProductId(products[8].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Xander Harris").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=xander").WithStars(4).WithComment("Looks modern and elegant.").Build(),
            new ProductRatingBuilder().WithProductId(products[9].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Yennefer of Vengerberg").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=yennefer").WithStars(4).WithComment("Good quality ceramic.").Build(),
            new ProductRatingBuilder().WithProductId(products[10].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Zoe Saldana").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=zoe").WithStars(5).WithComment("Beautiful design for living room.").Build(),
            new ProductRatingBuilder().WithProductId(products[11].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Arthur Morgan").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=arthur").WithStars(4).WithComment("Feels gentle on skin.").Build(),
            new ProductRatingBuilder().WithProductId(products[12].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Billie Eilish").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=billie").WithStars(5).WithComment("Skin became brighter after 2 weeks.").Build(),
            new ProductRatingBuilder().WithProductId(products[13].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Chris Evans").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=chris").WithStars(4).WithComment("Very moisturizing.").Build(),
            new ProductRatingBuilder().WithProductId(products[14].Id).WithCustomerId(Guid.NewGuid()).WithCustomerName("Dwayne Johnson").WithCustomerAvatarUrl("https://i.pravatar.cc/150?u=dwayne").WithStars(5).WithComment("Fits perfectly and stylish.").Build()
        };
        #endregion

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Products.AddRangeAsync(products);
        await dbContext.ProductRatings.AddRangeAsync(productRatings);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Database seeding completed.");
    }
}
