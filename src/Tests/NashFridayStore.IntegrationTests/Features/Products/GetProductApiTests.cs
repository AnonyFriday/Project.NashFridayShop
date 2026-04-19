using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.API.Features.Products.GetProduct;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;

namespace NashFridayStore.IntegrationTests.Features.Products;

public class GetProductApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly StoreDbContext _dbContext;
    private readonly IServiceScope _scope;

    public GetProductApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
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

    [Fact]
    public async Task GetProduct_ById_ShouldReturnProduct()
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
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result!.Id);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal(product.PriceUsd, result.PriceUsd);
        Assert.Equal(product.Status, result.Status);
        Assert.Equal(0.0m, result.AverageStars);
    }

    [Fact]
    public async Task GetProduct_WithRatings_ShouldReturnCorrectAverageStars()
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
            new ProductRatingBuilder().WithProductId(product.Id).WithStars(4).Build(),
            new ProductRatingBuilder().WithProductId(product.Id).WithStars(3).Build()
        ];

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        _dbContext.ProductRatings.AddRange(ratings);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result!.Id);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal(4.0m, result.AverageStars); // (5+4+3)/3 = 4
    }

    [Fact]
    public async Task GetProduct_WithDeletedRatings_ShouldExcludeDeletedRatingsFromAverage()
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
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result!.Id);
        Assert.Equal(5.0m, result.AverageStars); // Only the active rating (5) is counted
    }
}
