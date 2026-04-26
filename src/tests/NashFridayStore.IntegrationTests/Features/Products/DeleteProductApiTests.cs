using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;
using NashFridayStore.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.API.Features.Products.DeleteProduct;

namespace NashFridayStore.IntegrationTests.Features.Products;

public class DeleteProductApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly StoreDbContext _dbContext;

    public DeleteProductApiTests(CustomWebApplicationFactory factory)
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
    [Trait("DeleteProductApi", "ProductId")]
    [Fact]
    public async Task DeleteProduct_ProductNotFound_ShouldReturn404()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var productIdNotExists = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await _client.DeleteAsync(
            $"/api/products/{productIdNotExists}", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);
        Assert.NotNull(problemDetails);
        Assert.Equal("Product Not Found", problemDetails.Title);
        Assert.Contains(productIdNotExists.ToString(), problemDetails.Detail);
    }
    #endregion

    #region Soft Delete Tests
    [Trait("DeleteProductApi", "SoftDelete")]
    [Fact]
    public async Task DeleteProduct_ValidProductId_ShouldSoftDeleteProduct()
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

        Guid originalId = product.Id;

        // Act
        HttpResponseMessage response = await _client.DeleteAsync(
            $"/api/products/{product.Id}", ct);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: ct);

        Assert.NotNull(result);
        Assert.Equal(originalId, result!.Id);
        Assert.True(result.IsDeleted);
        Assert.True(result.DeletedAtUtc > DateTime.MinValue);

        // Check soft delete
        Product? deletedProduct = await _dbContext.Products
                                    .IgnoreQueryFilters() // ignore the default behavior to get the deleted products, due to difference dbContext
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == originalId && x.IsDeleted, ct);

        Assert.NotNull(deletedProduct);
        Assert.True(deletedProduct.IsDeleted);
        Assert.NotNull(deletedProduct.DeletedAtUtc);
    }

    [Trait("DeleteProductApi", "SoftDelete")]
    [Fact]
    public async Task DeleteProduct_DeletedProductNotRetrievable()
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
        await _client.DeleteAsync($"/api/products/{product.Id}", ct);

        // Act
        HttpResponseMessage response = await _client.GetAsync(
            $"/api/products/{product.Id}", ct);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Trait("DeleteProductApi", "SoftDelete")]
    [Fact]
    public async Task DeleteProduct_MultipleProducts_OnlyDeleteSpecified()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        Product product1 = new ProductBuilder().WithCategoryId(category.Id).Build();
        Product product2 = new ProductBuilder().WithCategoryId(category.Id).Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.AddRange(product1, product2);
        await _dbContext.SaveChangesAsync(ct);

        // Act
        HttpResponseMessage response = await _client.DeleteAsync(
            $"/api/products/{product1.Id}", ct);

        // Assert
        response.EnsureSuccessStatusCode();

        // Verify product1 is deleted
        Product? deletedProduct = await _dbContext.Products.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(x => x.Id == product1.Id && x.IsDeleted, ct);
        Assert.NotNull(deletedProduct);
        Assert.True(deletedProduct.IsDeleted);

        // Verify product2 is NOT deleted
        Product? activeProduct = await _dbContext.Products.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(x => x.Id == product2.Id && !x.IsDeleted, ct);
        Assert.NotNull(activeProduct);
        Assert.False(activeProduct.IsDeleted);
    }

    [Trait("DeleteProductApi", "SoftDelete")]
    [Fact]
    public async Task DeleteProduct_DeletedAtUtcShouldBeSet()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder().Build();
        Product product = new ProductBuilder().WithCategoryId(category.Id).Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(ct);

        DateTime beforeDelete = DateTime.UtcNow;

        // Act
        HttpResponseMessage response = await _client.DeleteAsync(
            $"/api/products/{product.Id}", ct);

        DateTime afterDelete = DateTime.UtcNow;

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: ct);

        Assert.NotNull(result);
        Assert.True(result!.DeletedAtUtc >= beforeDelete);
        Assert.True(result.DeletedAtUtc <= afterDelete);
    }
    #endregion
}
