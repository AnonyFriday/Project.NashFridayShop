namespace NashFridayStore.StoreFront.Services.Cart;

public class CartApiClient(BaseApiClient apiClient) : ICartApiClient
{
    private readonly BaseApiClient _apiClient = apiClient;

    public async Task<GetCart.Response?> GetCartAsync()
    {
        return await _apiClient.GetAsync<GetCart.Response>("api/customer/cart");
    }

    public async Task<CreateOrAddItemToCart.Response?> CreateOrAddItemAsync(CreateOrAddItemToCart.Request request)
    {
        return await _apiClient.PostAsync<CreateOrAddItemToCart.Request, CreateOrAddItemToCart.Response>("api/customer/cart", request);
    }
}
