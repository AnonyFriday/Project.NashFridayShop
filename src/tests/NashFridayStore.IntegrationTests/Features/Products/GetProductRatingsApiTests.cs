using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;
using NashFridayStore.API.Features.Products.GetProductRatings;

namespace NashFridayStore.IntegrationTests.Features.Products;

public class GetProductRatingsApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly StoreDbContext _dbContext;
    private readonly IServiceScope _scope;

    public GetProductRatingsApiTests(CustomWebApplicationFactory factory)
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
    public async Task GetProductRatings_WithRatings_ShouldReturnCorrectData()
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
            new ProductRatingBuilder()
                .WithProductId(product.Id)
                .WithStars(5)
                .WithComment("Excellent!")
                .WithCreatedAtUtc(DateTime.UtcNow.AddDays(-1))
                .Build(),
            new ProductRatingBuilder()
                .WithProductId(product.Id)
                .WithStars(4)
                .WithComment("Good product")
                .WithCreatedAtUtc(DateTime.UtcNow.AddDays(-2))
                .Build(),
            new ProductRatingBuilder()
                .WithProductId(product.Id)
                .WithStars(3)
                .WithComment(null)
                .WithCreatedAtUtc(DateTime.UtcNow.AddDays(-3))
                .Build()
        ];

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        _dbContext.ProductRatings.AddRange(ratings);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}/ratings", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(3, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(4.0m, result.Average); // (5+4+3)/3 = 4

        Assert.Equal(3, result.Items.Count);
        Assert.Equal(5, result.Items[0].Stars);
        Assert.Equal("Excellent!", result.Items[0].Comment);
        Assert.Equal(4, result.Items[1].Stars);
        Assert.Equal("Good product", result.Items[1].Comment);
        Assert.Equal(3, result.Items[2].Stars);
        Assert.Null(result.Items[2].Comment);
    }

    [Fact]
    public async Task GetProductRatings_NoRatings_ShouldReturnEmptyWithZeroAverage()
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
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}/ratings", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(0.0m, result.Average);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetProductRatings_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();

        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Laptop")
            .Build();

        var ratings = new ProductRating[5];
        for (int i = 0; i < 5; i++)
        {
            ratings[i] = new ProductRatingBuilder()
                .WithProductId(product.Id)
                .WithStars(5 - i)
                .WithComment($"Comment {i}")
                .WithCreatedAtUtc(DateTime.UtcNow.AddDays(-i))
                .Build();
        }

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        _dbContext.ProductRatings.AddRange(ratings);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}/ratings?pageIndex=1&pageSize=2", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(3, result.TotalPages); // 5 items / 2 per page = 3 pages
        Assert.Equal(1, result.PageIndex);
        Assert.Equal(3.0m, result.Average); // (5+4+3+2+1)/5 = 3

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(3, result.Items[0].Stars); // Second page: items 2 and 3 (0-indexed)
        Assert.Equal(2, result.Items[1].Stars);
    }

    [Fact]
    public async Task GetProductRatings_ProductNotFound_ShouldReturn404()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var nonExistentProductId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{nonExistentProductId}/ratings", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetProductRatings_WithDeletedRatings_ShouldExcludeDeletedRatings()
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
            new ProductRatingBuilder()
                .WithProductId(product.Id)
                .WithStars(5)
                .Build(),
            new ProductRatingBuilder()
                .WithProductId(product.Id)
                .WithStars(1)
                .WithIsDeleted(true)
                .Build()
        ];

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        _dbContext.ProductRatings.AddRange(ratings);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}/ratings", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(1, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(5.0m, result.Average); // Only the active rating (5) is counted

        Assert.Single(result.Items);
        Assert.Equal(5, result.Items[0].Stars);
    }

    [Fact]
    public async Task GetProductRatings_InvalidPageSize_ShouldReturnValidationError()
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
        HttpResponseMessage response = await _client.GetAsync($"/api/products/{product.Id}/ratings?pageSize=150", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
