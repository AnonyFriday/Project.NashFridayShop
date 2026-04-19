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
    }
}
