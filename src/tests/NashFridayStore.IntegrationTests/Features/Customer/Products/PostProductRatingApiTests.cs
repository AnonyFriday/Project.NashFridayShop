using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Builders;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.IntegrationTests.Commons;
using NashFridayStore.API.Features.Customer.Products.PostProductRating;
using NashFridayStore.Domain.Entities.Categories;

namespace NashFridayStore.IntegrationTests.Features.Customer.Products;

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
            $"/api/customer/products/{productIdNotExists}/rating", request, cancellationToken);

        // Assert
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: cancellationToken);

        Assert.NotNull(problemDetails);
        Assert.Equal("Product Not Found", problemDetails.Title);
        Assert.Contains(productIdNotExists.ToString(), problemDetails.Detail);
    }

    [Trait("PostProductRatingApi", "ProductId")]
    [Fact]
    public async Task PostProductRating_CustomerId_AlreadyRateAProduct()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        var testUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        Category category = new CategoryBuilder().Build();
        Product product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .Build();

        ProductRating existingRating = new ProductRatingBuilder()
            .WithStars(4)
            .WithComment("I Love You")
            .WithCustomerId(testUserId)
            .WithProductId(product.Id)
            .Build();

        _dbContext.Categories.Add(category);
        _dbContext.Products.Add(product);
        _dbContext.ProductRatings.Add(existingRating);
        await _dbContext.SaveChangesAsync(ct);

        // another rating
        var request = new RequestBody("Hate You", 1);

        // Act
        // - since we already create the claim as 000-000, when calling this api, 1 user rates the same thing
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            $"/api/customer/products/{product.Id}/rating", request, ct);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: ct);

        Assert.NotNull(problemDetails);
        Assert.Equal("Customer already rated the product", problemDetails.Title);
        Assert.Contains(product.Name, problemDetails.Detail);
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

        var request = new RequestBody("Amazing product!", 5);

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            $"/api/customer/products/{product.Id}/rating",
            request,
            ct
        );

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Response? result = await response.Content.ReadFromJsonAsync<Response>(CustomWebApplicationFactory.DefaultJsonOptions, cancellationToken: ct);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result!.ProductId);
        Assert.Equal(5, result.Stars);
        Assert.Equal("Amazing product!", result.Comment);

        ProductRating? ratingInDb = _dbContext.ProductRatings.FirstOrDefault();

        Assert.NotNull(ratingInDb);
        Assert.Equal(product.Id, ratingInDb!.ProductId);
        Assert.Equal(5, ratingInDb.Stars);
    }
    #endregion
}
