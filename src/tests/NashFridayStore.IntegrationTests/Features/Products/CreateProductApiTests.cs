using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;
using NashFridayStore.API.Features.Products.CreateProduct;

namespace NashFridayStore.IntegrationTests.Features.Products;

public class CreateProductApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly StoreDbContext _dbContext;

    public CreateProductApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        // New scope created and delete database per test to ensure isolation
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

    #region CategoryId Tests
    [Trait("CreateProductApi", "CategoryId")]
    [Fact]
    public async Task CreateProduct_CategoryNotFound_ShouldReturn404()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var categoryIdNotExists = Guid.NewGuid();
        var request = new Request(
            categoryIdNotExists,
            "New Product",
            "Description",
            99.99m,
            "https://image.url",
            10);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/products", request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);
        Assert.NotNull(problemDetails);
        Assert.Equal("Category Not Found", problemDetails.Title);
        Assert.Contains(categoryIdNotExists.ToString(), problemDetails.Detail);
    }
    #endregion

    #region Validation Tests
    [Trait("CreateProductApi", "Validation")]
    [Fact]
    public async Task CreateProduct_EmptyName_ShouldReturn400()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var request = new Request(
            category.Id,
            string.Empty,
            "Description",
            99.99m,
            "https://image.url",
            10);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/products", request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Trait("CreateProductApi", "Validation")]
    [Fact]
    public async Task CreateProduct_PriceIsZero_ShouldReturn400()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var request = new Request(
            category.Id,
            "Product Name",
            "Description",
            0,
            "https://image.url",
            10);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/products", request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Trait("CreateProductApi", "Validation")]
    [Fact]
    public async Task CreateProduct_NegativeQuantity_ShouldReturn400()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var request = new Request(
            category.Id,
            "Product Name",
            "Description",
            99.99m,
            "https://image.url",
            -1);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/products", request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    #endregion

    #region Success Tests
    [Trait("CreateProductApi", "Success")]
    [Fact]
    public async Task CreateProduct_ValidRequest_ShouldCreateProduct()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(ct);

        var request = new Request(
            category.Id,
            "Laptop",
            "High-performance laptop",
            1299.99m,
            "https://image.url",
            5,
            ProductStatus.InStock);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/products", request, ct);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: ct);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result!.Id);
        Assert.Equal(request.CategoryId, result.CategoryId);
        Assert.Equal("Laptop", result.Name);
        Assert.Equal("High-performance laptop", result.Description);
        Assert.Equal(1299.99m, result.PriceUsd);
        Assert.Equal(5, result.Quantity);
        Assert.Equal(ProductStatus.InStock, result.Status);
        Assert.True(result.CreatedAtUtc > DateTime.MinValue);
    }

    [Trait("CreateProductApi", "Success")]
    [Fact]
    public async Task CreateProduct_MultipleProducts_ShouldCreateAllProducts()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(ct);

        var request1 = new Request(category.Id, "Product 1", "Description 1", 10.99m, "https://image.url", 5);
        var request2 = new Request(category.Id, "Product 2", "Description 2", 20.99m, "https://image.url", 10);

        // Act
        HttpResponseMessage response1 = await _client.PostAsJsonAsync("/api/products", request1, ct);
        HttpResponseMessage response2 = await _client.PostAsJsonAsync("/api/products", request2, ct);

        // Assert
        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();

        Response? result1 = await response1.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: ct);
        Response? result2 = await response2.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: ct);

        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.NotEqual(result1!.Id, result2!.Id);
        Assert.Equal("Product 1", result1.Name);
        Assert.Equal("Product 2", result2.Name);

        int productCount = _dbContext.Products.Count();
        Assert.Equal(2, productCount);
    }

    [Trait("CreateProductApi", "Success")]
    [Fact]
    public async Task CreateProduct_WithDefaultStatus_ShouldCreateWithInStockStatus()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(ct);

        var request = new Request(
            category.Id,
            "Product",
            "Description",
            99.99m,
            "https://image.url",
            5);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/products", request, ct);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: ct);

        Assert.NotNull(result);
        Assert.Equal(ProductStatus.InStock, result!.Status);
    }
    #endregion
}
