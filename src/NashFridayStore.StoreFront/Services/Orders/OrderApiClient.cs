namespace NashFridayStore.StoreFront.Services.Orders;

public class OrderApiClient : IOrderApiClient
{
    private readonly BaseApiClient _apiClient;

    public OrderApiClient(BaseApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CreateCheckout.Response?> CreateCheckoutAsync(CreateCheckout.Request request)
    {
        return await _apiClient.PostAsync<CreateCheckout.Request, CreateCheckout.Response>("api/customer/orders/checkout", request);
    }
}
