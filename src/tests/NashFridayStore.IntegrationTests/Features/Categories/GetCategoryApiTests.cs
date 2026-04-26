using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.API.Features.Categories.GetCategory;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;

namespace NashFridayStore.IntegrationTests.Features.Categories;

public class GetCategoryApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly StoreDbContext _dbContext;
    private readonly IServiceScope _scope;

    public GetCategoryApiTests(CustomWebApplicationFactory factory)
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
    public async Task GetCategory_ById_ShouldReturnCategory()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder()
            .WithName("Electronics")
            .WithDescription("Electronics and gadgets")
            .Build();

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories/{category.Id}", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result!.Id);
        Assert.Equal("Electronics", result.Name);
        Assert.Equal("Electronics and gadgets", result.Description);
    }

    [Fact]
    public async Task GetCategory_NonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var nonExistentId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/api/categories/{nonExistentId}", cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
