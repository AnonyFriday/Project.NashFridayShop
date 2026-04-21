using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.API.Features.Products.PostProductRating;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;

namespace NashFridayStore.IntegrationTests.Features.Products;

public class PostProductRatingApiTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly StoreDbContext _dbContext;

    public PostProductRatingApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        // New scope created and delete databae per test to ensure isolcation
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

    #region ProductId
    [Trait("PostProductRatingApi", "ProductId")]
    [Fact]
    public async Task PostProductRating_ProductId_ProductNotFound()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        var productIdNotExists = Guid.NewGuid();
        var request = new RequestBody("I Love You", 3);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            $"/api/products/{productIdNotExists}/rating", request, cancellationToken);

        // Assert
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);

        Assert.NotNull(problemDetails);
        Assert.Equal("Product not found", problemDetails.Title);
        Assert.Contains(productIdNotExists.ToString(), problemDetails.Detail);
    }
    #endregion

    #region Success
    [Trait("PostProductRatingApi", "Success")]
    [Fact]
    public async Task PostProductRating_ShouldCreateRating()
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

        var request = new RequestBody("Amazing product!", 9);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            $"/api/products/{product.Id}/rating",
            request,
            ct
        );

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Response? result = await response.Content.ReadFromJsonAsync<Response>(cancellationToken: ct);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result!.ProductId);
        Assert.Equal(9, result.Stars);
        Assert.Equal("Amazing product!", result.Comment);

        ProductRating? ratingInDb = _dbContext.ProductRatings.FirstOrDefault();

        Assert.NotNull(ratingInDb);
        Assert.Equal(product.Id, ratingInDb!.ProductId);
        Assert.Equal(9, ratingInDb.Stars);
    }
    #endregion
}
