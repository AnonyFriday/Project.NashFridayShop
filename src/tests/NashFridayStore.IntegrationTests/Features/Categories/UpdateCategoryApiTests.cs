using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.API.Features.Categories.UpdateCategory;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;

namespace NashFridayStore.IntegrationTests.Features.Categories;

public class UpdateCategoryApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly StoreDbContext _dbContext;
    private readonly IServiceScope _scope;

    public UpdateCategoryApiTests(CustomWebApplicationFactory factory)
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
    [Trait("IT", "UpdateCategory")]
    public async Task UpdateCategory_ValidRequest_ShouldReturnOk()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category = new CategoryBuilder()
            .WithName("Old Name")
            .WithDescription("Old Description")
            .Build();

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var body = new RequestBody("New Name", "New Description");

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync($"/api/categories/{category.Id}", body, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal("New Name", result!.Name);
        Assert.Equal("New Description", result.Description);

        // Verify in DB
        Category? categoryInDb = await _dbContext.Categories
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == category.Id, cancellationToken);
        Assert.NotNull(categoryInDb);
        Assert.Equal("New Name", categoryInDb!.Name);
    }

    [Fact]
    [Trait("IT", "UpdateCategory")]
    public async Task UpdateCategory_NonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var nonExistentId = Guid.NewGuid();
        var body = new RequestBody("Name", "Description");

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync($"/api/categories/{nonExistentId}", body, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    [Trait("IT", "UpdateCategory")]
    public async Task UpdateCategory_DuplicateName_ShouldReturnConflict()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category1 = new CategoryBuilder().WithName("Cat 1").Build();
        Category category2 = new CategoryBuilder().WithName("Cat 2").Build();

        _dbContext.Categories.AddRange(category1, category2);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var body = new RequestBody("Cat 2", "Description"); // Try to rename Cat 1 to Cat 2

        // Act
        HttpResponseMessage response = await _client.PutAsJsonAsync($"/api/categories/{category1.Id}", body, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
