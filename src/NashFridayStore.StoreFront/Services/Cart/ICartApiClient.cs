namespace NashFridayStore.StoreFront.Services.Cart;

public interface ICartApiClient
{
    Task<GetCart.Response?> GetCartAsync();
    Task<CreateOrAddItemToCart.Response?> CreateOrAddItemAsync(CreateOrAddItemToCart.Request request);
}
