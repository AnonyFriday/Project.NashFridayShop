using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;
using NashFridayStore.API.Features.Products.UpdateProduct;

namespace NashFridayStore.IntegrationTests.Features.Products;

public class UpdateProductApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly StoreDbContext _dbContext;

    public UpdateProductApiTests(CustomWebApplicationFactory factory)
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

    #region Product Not Found Tests
    [Trait("UpdateProductApi", "ProductId")]
    [Fact]
    public async Task UpdateProduct_ProductNotFound_ShouldReturn404()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var productIdNotExists = Guid.NewGuid();
        var body = new RequestBody(
            category.Id,
            "Updated Name",
            "Updated Description",
            199.99m,
            "https://image.url",
            20,
            ProductStatus.InStock);

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/products/{productIdNotExists}", body, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);
        Assert.NotNull(problemDetails);
        Assert.Equal("Product Not Found", problemDetails.Title);
    }
    #endregion

    #region Category Not Found Tests
    [Trait("UpdateProductApi", "CategoryId")]
    [Fact]
    public async Task UpdateProduct_CategoryNotFound_ShouldReturn404()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var categoryIdNotExists = Guid.NewGuid();
        var body = new RequestBody(
            categoryIdNotExists,
            "Updated Name",
            "Updated Description",
            199.99m,
            "https://image.url",
            20,
            ProductStatus.InStock);

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/products/{product.Id}", body, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    #endregion

    #region Validation Tests
    [Trait("UpdateProductApi", "Validation")]
    [Fact]
    public async Task UpdateProduct_EmptyName_ShouldReturn400()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var body = new RequestBody(
            category.Id,
            string.Empty,
            "Updated Description",
            199.99m,
            "https://image.url",
            20,
            ProductStatus.InStock);

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/products/{product.Id}", body, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Trait("UpdateProductApi", "Validation")]
    [Fact]
    public async Task UpdateProduct_PriceIsZero_ShouldReturn400()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var body = new RequestBody(
            category.Id,
            "Updated Name",
            "Updated Description",
            0,
            "https://image.url",
            20,
            ProductStatus.InStock);

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/products/{product.Id}", body, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    #endregion

    #region Success Tests
    [Trait("UpdateProductApi", "Success")]
    [Fact]
    public async Task UpdateProduct_ValidRequest_ShouldUpdateProduct()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithName("Original Name")
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(ct);

        var body = new RequestBody(
            category.Id,
            "Updated Name",
            "Updated Description",
            199.99m,
            "https://updated-image.url",
            20,
            ProductStatus.OutOfStock);

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/products/{product.Id}", body, ct);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: ct);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result!.Id);
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal("Updated Description", result.Description);
        Assert.Equal(199.99m, result.PriceUsd);
        Assert.Equal(20, result.Quantity);
        Assert.Equal(ProductStatus.OutOfStock, result.Status);
        Assert.True(result.UpdatedAtUtc > DateTime.UtcNow.AddSeconds(-5));
    }

    [Trait("UpdateProductApi", "Success")]
    [Fact]
    public async Task UpdateProduct_UpdatedAtUtcShouldBeSet()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(ct);

        DateTime? originalUpdatedAt = product.UpdatedAtUtc;

        var body = new RequestBody(
            category.Id,
            "Updated Name",
            "Updated Description",
            99.99m,
            "https://image.url",
            5,
            ProductStatus.InStock);

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/products/{product.Id}", body, ct);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: ct);

        Assert.NotNull(result);
        Assert.NotEqual(originalUpdatedAt, result!.UpdatedAtUtc);
    }

    [Trait("UpdateProductApi", "Success")]
    [Fact]
    public async Task UpdateProduct_DeletedProductShouldNotBeUpdated()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .WithIsDeleted(true)
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(ct);

        var body = new RequestBody(
            category.Id,
            "Updated Name",
            "Updated Description",
            99.99m,
            "https://image.url",
            5,
            ProductStatus.InStock);

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/products/{product.Id}", body, ct);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    #endregion
}


