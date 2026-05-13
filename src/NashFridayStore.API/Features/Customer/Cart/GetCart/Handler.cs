using NashFridayStore.API.Auth;
using NashFridayStore.Domain.Entities.Carts;
using NashFridayStore.Infrastructure.Interfaces;

namespace NashFridayStore.API.Features.Customer.Cart.GetCart;

public sealed class Handler(
    ICartService cartService,
    ICurrentUser currentUser)
{
    public async Task<Response> HandleAsync(CancellationToken ct)
    {
        ShoppingCart? cart = await cartService.GetCartAsync<ShoppingCart>(currentUser.Id);

        if (cart == null)
        {
            return new Response(
                CustomerId: currentUser.Id,
                CustomerName: currentUser.Name,
                DeliveryAddress: string.Empty,
                Currency: "USD",
                TotalPriceInUsd: 0,
                UpdatedAtUtc: null,
                Items: []);
        }

        return new Response(
            cart.CustomerId,
            cart.CustomerName,
            cart.DeliveryAddress,
            cart.Currency,
            cart.TotalPriceInUsd,
            cart.UpdatedAtUtc,
            cart.Items.Values.Select(i => new ShoppingCartItemResponse(
                i.ProductId,
                i.ProductName,
                i.PriceInUsd,
                i.Quantity,
                i.ImageUrl,
                i.CategoryId,
                i.CategoryName
            )).ToList());
    }
}
