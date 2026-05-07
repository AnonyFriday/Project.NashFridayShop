namespace NashFridayStore.StoreFront.Services.Products;

public interface IProductApiClient
{
    Task<GetProducts.Response?> GetProductsAsync(GetProducts.Request request);
    Task<GetTopRatedProducts.Response?> GetTopRatedProductsAsync(GetTopRatedProducts.Request request);
}
