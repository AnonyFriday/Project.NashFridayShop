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

namespace NashFridayStore.IntegrationTests.Features.Products;

public class ToggleToggleDeleteProductApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly StoreDbContext _dbContext;

    public ToggleToggleDeleteProductApiTests(CustomWebApplicationFactory factory)
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
    [Trait("ToggleDeleteProductApi", "Id")]
    [Fact]
    public async Task ToggleDeleteProduct_ProductNotFound_ShouldReturn404()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var productIdNotExists = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await _client.PatchAsync(
            $"/api/products/{productIdNotExists}", null, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);
        Assert.NotNull(problemDetails);
        Assert.Equal("Product Not Found", problemDetails.Title);
        Assert.Contains(productIdNotExists.ToString(), problemDetails.Detail);
    }
    #endregion

    #region Toggle Delete Tests
    [Trait("ToggleDeleteProductApi", "ToggleDelete")]
    [Fact]
    public async Task ToggleDeleteProduct_DeletedProductNotRetrievableByDefault()
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
        await _client.PatchAsync($"/api/products/{product.Id}", null, ct);

        // Act
        HttpResponseMessage response = await _client.GetAsync(
            $"/api/products/{product.Id}", ct);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Trait("ToggleDeleteProductApi", "ToggleDelete")]
    [Fact]
    public async Task ToggleDeleteProduct_MultipleProducts_OnlyToggleSpecified()
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
        HttpResponseMessage response = await _client.PatchAsync(
            $"/api/products/{product1.Id}", null, ct);

        // Assert
        response.EnsureSuccessStatusCode();

        // Verify product1 is toggled to deleted
        Product? deletedProduct = await _dbContext.Products.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(x => x.Id == product1.Id && x.IsDeleted, ct);
        Assert.NotNull(deletedProduct);
        Assert.True(deletedProduct.IsDeleted);

        // Verify product2 is NOT deleted
        Product? activeProduct = await _dbContext.Products.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(x => x.Id == product2.Id && !x.IsDeleted, ct);
        Assert.NotNull(activeProduct);
        Assert.False(activeProduct.IsDeleted);
    }
    #endregion
}
