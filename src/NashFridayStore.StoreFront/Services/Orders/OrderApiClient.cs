namespace NashFridayStore.StoreFront.Services.Orders;

public sealed class OrderApiClient(BaseApiClient apiClient) : IOrderApiClient
{
    public async Task<CreateCheckout.Response?> CreateCheckoutAsync(CreateCheckout.Request request)
    {
        return await apiClient.PostAsync<CreateCheckout.Request, CreateCheckout.Response>("api/customer/orders/checkout", request);
    }

    public async Task<GetMyOrders.Response?> GetMyOrdersAsync()
    {
        return await apiClient.GetAsync<GetMyOrders.Response>("api/customer/orders");
    }
}
