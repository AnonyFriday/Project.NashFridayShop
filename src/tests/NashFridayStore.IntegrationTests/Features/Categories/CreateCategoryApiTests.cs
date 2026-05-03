using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.API.Features.Categories.CreateCategory;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;

namespace NashFridayStore.IntegrationTests.Features.Categories;

public class CreateCategoryApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly StoreDbContext _dbContext;
    private readonly IServiceScope _scope;

    public CreateCategoryApiTests(CustomWebApplicationFactory factory)
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
    [Trait("IT", "CreateCategory")]
    public async Task CreateCategory_ValidRequest_ShouldReturnCreated()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var request = new Request("Electronics", "Category for gadgets");

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/categories", request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal("Electronics", result!.Name);
        Assert.Equal("Category for gadgets", result.Description);
    }

    [Fact]
    [Trait("IT", "CreateCategory")]
    public async Task CreateCategory_DuplicateName_ShouldReturnConflict()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category existingCategory = new CategoryBuilder()
            .WithName("Duplicate")
            .Build();

        _dbContext.Categories.Add(existingCategory);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var request = new Request("Duplicate", "Another description");

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/categories", request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    [Trait("IT", "CreateCategory")]
    public async Task CreateCategory_InvalidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var request = new Request("", "Description too long".PadRight(301, 'a'));

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/categories", request, cancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
