using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;
using NashFridayStore.API.Features.Categories.GetCategories;

namespace NashFridayStore.IntegrationTests.Features.Categories;

public class GetCategoriesApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly StoreDbContext _dbContext;
    private readonly IServiceScope _scope;

    public GetCategoriesApiTests(CustomWebApplicationFactory factory)
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
    public async Task GetCategories_ShouldReturnCategories()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category1 = new CategoryBuilder()
            .WithName("Electronics")
            .WithDescription("Electronic devices")
            .Build();

        Category category2 = new CategoryBuilder()
            .WithName("Books")
            .WithDescription("Books and literature")
            .Build();

        _dbContext.Categories.AddRange(category1, category2);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/categories", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(2, result.Items.Count);

        CategoryItem electronics = result.Items.First(c => c.Name == "Electronics");
        Assert.Equal(category1.Id, electronics.Id);
        Assert.Equal("Electronics", electronics.Name);
        Assert.Equal("Electronic devices", electronics.Description);

        CategoryItem books = result.Items.First(c => c.Name == "Books");
        Assert.Equal(category2.Id, books.Id);
        Assert.Equal("Books", books.Name);
        Assert.Equal("Books and literature", books.Description);
    }

    [Fact]
    public async Task GetCategories_WithSearchName_ShouldReturnFilteredCategories()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Category category1 = new CategoryBuilder()
            .WithName("Electronics")
            .WithDescription("Electronic devices")
            .Build();

        Category category2 = new CategoryBuilder()
            .WithName("Books")
            .WithDescription("Books and literature")
            .Build();

        _dbContext.Categories.AddRange(category1, category2);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/categories?searchName=Book", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(1, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(0, result.PageIndex);
        Assert.Single(result.Items);

        CategoryItem books = result.Items[0];
        Assert.Equal(category2.Id, books.Id);
        Assert.Equal("Books", books.Name);
        Assert.Equal("Books and literature", books.Description);
    }

    [Fact]
    public async Task GetCategories_WithPagination_ShouldReturnPagedCategories()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var categories = new List<Category>();
        for (int i = 1; i <= 5; i++)
        {
            categories.Add(new CategoryBuilder()
                .WithName($"Category {i}")
                .WithDescription($"Description {i}")
                .Build());
        }

        _dbContext.Categories.AddRange(categories);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/categories?pageSize=2&pageIndex=1", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(1, result.PageIndex);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task GetCategories_WithIsAllTrue_ShouldReturnAllCategoriesWithoutPagination()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var categories = new List<Category>();
        for (int i = 1; i <= 5; i++)
        {
            categories.Add(new CategoryBuilder()
                .WithName($"Category {i}")
                .WithDescription($"Description {i}")
                .Build());
        }

        _dbContext.Categories.AddRange(categories);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/categories?isAll=true", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(5, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(5, result.Items.Count);
    }

    [Fact]
    public async Task GetCategories_WithIsAllTrueAndSearchName_ShouldReturnFilteredCategoriesWithoutPagination()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var categories = new List<Category>();
        for (int i = 1; i <= 5; i++)
        {
            string name = i <= 2 ? $"Book {i}" : $"Category {i}";
            categories.Add(new CategoryBuilder()
                .WithName(name)
                .WithDescription($"Description {i}")
                .Build());
        }

        _dbContext.Categories.AddRange(categories);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/categories?isAll=true&searchName=Book", cancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(2, result.Items.Count);
    }
}
