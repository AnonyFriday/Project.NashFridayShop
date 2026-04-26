using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;
using NashFridayStore.API.Features.Products.GetProducts;

namespace NashFridayStore.IntegrationTests.Features.Products;

public class GetProductsApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly StoreDbContext _dbContext;
    private readonly IServiceScope _scope;

    public GetProductsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        // New scope created and delete databae per test to ensure isolcation
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<StoreDbContext>();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _scope.Dispose();
        _client.Dispose();
    }

    #region SearchName Tests
    [Fact]
    public async Task GetProducts_SearchName_ShouldFilterCorrectly()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder()
            .WithName("Electronics")
            .Build();

        Product[] products =
        [
            new ProductBuilder()
                .WithCategoryId(category.Id)
                .WithName("Laptop")
                .Build(),

            new ProductBuilder()
                .WithCategoryId(category.Id)
                .WithName("Mouse")
                .Build()
        ];

        _dbContext.Categories.AddRange(category);
        _dbContext.Products.AddRange(products);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/products?searchName=Lap", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result.ProductItems);
        Assert.Equal("Laptop", result.ProductItems[0].Name);
    }

    [Fact]
    public async Task GetProducts_SearchName_NoMatch_ShouldReturnEmpty()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();

        _dbContext.Products.AddRange(
            new ProductBuilder().WithCategoryId(category.Id).WithName("Laptop").Build(),
            new ProductBuilder().WithCategoryId(category.Id).WithName("Mouse").Build()
        );

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/products?searchName=Phone", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken);

        Assert.NotNull(result);
        Assert.Empty(result.ProductItems);
    }
    #endregion

    #region CategoryId Tests
    [Fact]
    public async Task GetProducts_CategoryId_ShouldFilterCorrectly()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category cat1 = new CategoryBuilder().WithName("Electronics").Build();
        Category cat2 = new CategoryBuilder().WithName("Food").Build();

        _dbContext.Categories.AddRange(cat1, cat2);
        _dbContext.Products.AddRange(
            new ProductBuilder().WithCategoryId(cat1.Id).WithName("Laptop").Build(),
            new ProductBuilder().WithCategoryId(cat2.Id).WithName("Apple").Build()
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products?categoryId={cat1.Id}", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.Single(result!.ProductItems);
        Assert.Equal("Laptop", result.ProductItems[0].Name);
    }

    [Fact]
    public async Task GetProducts_CategoryId_NoMatch_ShouldReturnEmpty()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category cat = new CategoryBuilder().Build();
        _dbContext.Categories.Add(cat);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products?categoryId={Guid.NewGuid()}", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.Empty(result!.ProductItems);
    }
    #endregion

    #region Price Range Tests
    [Fact]
    public async Task GetProducts_PriceRange_ShouldFilterCorrectly()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.AddRange(
            new ProductBuilder().WithCategoryId(category.Id).WithPrice(10).Build(),
            new ProductBuilder().WithCategoryId(category.Id).WithPrice(100).Build()
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/products?minPrice=50&MaxPrice=150", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result.ProductItems);
        Assert.Equal(100, result.ProductItems[0].PriceUsd);
    }

    [Fact]
    public async Task GetProducts_MinPriceOnly_ShouldFilterCorrectly()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.AddRange(
            new ProductBuilder().WithCategoryId(category.Id).WithPrice(10).Build(),
            new ProductBuilder().WithCategoryId(category.Id).WithPrice(100).Build()
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/products?minPrice=50", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result.ProductItems);
        Assert.Equal(100, result.ProductItems[0].PriceUsd);
    }
    #endregion

    #region IsDeleted and Status Tests

    [Fact]
    public async Task GetProducts_ShouldExcludeDeletedProducts()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Category category = new CategoryBuilder().Build();

        Product active = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Laptop")
            .WithIsDeleted(false)
            .Build();

        Product deleted = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Mouse")
            .WithIsDeleted(true)
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.AddRange(active, deleted);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/products", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result.ProductItems);
        Assert.Equal("Laptop", result.ProductItems[0].Name);
    }

    [Fact]
    public async Task GetProducts_Status_ShouldFilterCorrectly()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Category category = new CategoryBuilder().Build();

        Product inStock = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Laptop")
            .WithStatus(ProductStatus.InStock)
            .Build();

        Product outOfStock = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Mouse")
            .WithStatus(ProductStatus.OutOfStock)
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.AddRange(inStock, outOfStock);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products?status=InStock", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();

        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result.ProductItems);
        Assert.Equal(ProductStatus.InStock, result.ProductItems[0].Status);
    }

    #endregion

    #region AverageStars Tests
    [Fact]
    public async Task GetProducts_WithRatings_ShouldReturnCorrectAverageStars()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();

        Product product1 = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Laptop")
            .Build();

        Product product2 = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Mouse")
            .Build();

        ProductRating[] ratings1 =
        [
            new ProductRatingBuilder().WithProductId(product1.Id).WithStars(5).Build(),
            new ProductRatingBuilder().WithProductId(product1.Id).WithStars(4).Build(),
            new ProductRatingBuilder().WithProductId(product1.Id).WithStars(3).Build()
        ];

        ProductRating[] ratings2 =
        [
            new ProductRatingBuilder().WithProductId(product2.Id).WithStars(5).Build(),
            new ProductRatingBuilder().WithProductId(product2.Id).WithStars(5).Build()
        ];

        _dbContext.Categories.Add(category);
        _dbContext.Products.AddRange(product1, product2);
        _dbContext.ProductRatings.AddRange(ratings1);
        _dbContext.ProductRatings.AddRange(ratings2);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/products", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.ProductItems.Count);

        ProductItem laptopItem = result.ProductItems.First(p => p.Name == "Laptop");
        ProductItem mouseItem = result.ProductItems.First(p => p.Name == "Mouse");

        Assert.Equal(4.0m, laptopItem.AverageStars); // (5+4+3)/3 = 4
        Assert.Equal(5.0m, mouseItem.AverageStars); // (5+5)/2 = 5
    }

    [Fact]
    public async Task GetProducts_NoRatings_ShouldReturnZeroAverageStars()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();

        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Laptop")
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/products", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result.ProductItems);
        Assert.Equal(0.0m, result.ProductItems[0].AverageStars);
    }

    [Fact]
    public async Task GetProducts_WithDeletedRatings_ShouldExcludeDeletedRatingsFromAverage()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();

        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Laptop")
            .Build();

        ProductRating[] ratings =
        [
            new ProductRatingBuilder().WithProductId(product.Id).WithStars(5).Build(),
            new ProductRatingBuilder().WithProductId(product.Id).WithStars(1).WithIsDeleted(true).Build()
        ];

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        _dbContext.ProductRatings.AddRange(ratings);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/products", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Single(result.ProductItems);
        Assert.Equal(5.0m, result.ProductItems[0].AverageStars); // Only the active rating (5) is counted
    }
    #endregion
}
