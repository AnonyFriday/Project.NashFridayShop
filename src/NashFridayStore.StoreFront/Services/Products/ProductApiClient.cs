namespace NashFridayStore.StoreFront.Services.Products;

public class ProductApiClient : IProductApiClient
{
    private readonly BaseApiClient _apiClient;

    public ProductApiClient(BaseApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetProducts.Response?> GetProductsAsync(GetProducts.Request request)
    {
        var queryParams = new List<string>();
        if (request.CategoryId.HasValue)
        {
            queryParams.Add($"categoryId={request.CategoryId.Value}");
        }
        if (!string.IsNullOrWhiteSpace(request.SearchName))
        {
            queryParams.Add($"searchName={request.SearchName}");
        }
        if (request.MinPrice.HasValue)
        {
            queryParams.Add($"minPrice={request.MinPrice.Value}");
        }
        if (request.MaxPrice.HasValue)
        {
            queryParams.Add($"maxPrice={request.MaxPrice.Value}");
        }
        if (request.Status.HasValue)
        {
            queryParams.Add($"status={request.Status.Value}");
        }
        if (request.SortBy.HasValue)
        {
            queryParams.Add($"sortBy={request.SortBy.Value}");
        }
        queryParams.Add($"pageIndex={request.PageIndex}");
        queryParams.Add($"pageSize={request.PageSize}");

        string queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
        return await _apiClient.GetAsync<GetProducts.Response>($"api/all/products{queryString}");
    }

    public async Task<GetTopRatedProducts.Response?> GetTopRatedProductsAsync(GetTopRatedProducts.Request request)
    {
        GetTopRatedProducts.Response? response = await _apiClient.GetAsync<GetTopRatedProducts.Response>($"api/all/products?pageSize={request.Count}&sortBy=RatingDesc");

        if (response?.Items == null || !response.Items.Any())
        {
            return new GetTopRatedProducts.Response(new List<GetTopRatedProducts.ProductItem>());
        }

        return response;
    }

    public async Task<GetProduct.Response?> GetProductByIdAsync(GetProduct.Request request)
    {
        return await _apiClient.GetAsync<GetProduct.Response>($"api/all/products/{request.Id}");
    }

    public async Task<GetProductRatings.Response?> GetProductRatingsAsync(GetProductRatings.Request request)
    {
        return await _apiClient.GetAsync<GetProductRatings.Response>($"api/all/products/{request.ProductId}/ratings?pageIndex={request.PageIndex}&pageSize={request.PageSize}");
    }
}
