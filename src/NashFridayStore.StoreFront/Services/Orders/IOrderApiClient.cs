namespace NashFridayStore.StoreFront.Services.Orders;

public interface IOrderApiClient
{
    Task<CreateCheckout.Response?> CreateCheckoutAsync(CreateCheckout.Request request);
}
