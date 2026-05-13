namespace NashFridayStore.StoreFront.Services.Products;

public interface IProductApiClient
{
    Task<GetProducts.Response?> GetProductsAsync(GetProducts.Request request);
    Task<GetTopRatedProducts.Response?> GetTopRatedProductsAsync(GetTopRatedProducts.Request request);
    Task<GetProduct.Response?> GetProductByIdAsync(GetProduct.Request request);
    Task<GetProductRatings.Response?> GetProductRatingsAsync(GetProductRatings.Request request);
    Task<PostProductRating.Response?> PostProductRatingAsync(PostProductRating.Request request);
}
