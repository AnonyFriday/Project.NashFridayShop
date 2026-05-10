using FluentValidation;
using FluentValidation.Results;
using NashFridayStore.API.Auth;
using NashFridayStore.Domain.Entities.Carts;
using NashFridayStore.Infrastructure.Interfaces;

namespace NashFridayStore.API.Features.Customer.Cart.CreateOrAddItemToCart;

public sealed class Handler(
    IValidator<Request> validator,
    ICartService cartService,
    ICurrentUser currentUser)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Fetch existing cart
        ShoppingCart? cart = await cartService.GetCartAsync<ShoppingCart>(currentUser.Id);

        // Make sure the first item always > 0 to create a cart
        if (cart == null && req.Quantity > 0)
        {
            // No cart exists - Create new one
            var newItem = new ShoppingCartItem
            {
                ProductId = req.ProductId,
                ProductName = req.ProductName,
                PriceInUsd = req.Price,
                Quantity = req.Quantity,
                ImageUrl = req.ImageUrl
            };

            cart = new ShoppingCart
            {
                CustomerId = currentUser.Id,
                UpdatedAtUtc = DateTime.UtcNow,
                Items = { [req.ProductId] = newItem }
            };

            await cartService.SetCartAsync(currentUser.Id, cart);
        }
        else if (cart != null)
        {
            // Cart exists - Add or Update item
            if (cart.Items.TryGetValue(req.ProductId, out ShoppingCartItem? item))
            {
                int newQuantity = item.Quantity + req.Quantity;

                // if new quantity <= 0, remove from cart
                if (newQuantity <= 0)
                {
                    await cartService.DeletePartialCartAsync(currentUser.Id, $"$.items['{req.ProductId}']");
                    cart.Items.Remove(req.ProductId);
                }
                else
                {
                    // Update item surgically
                    item.Quantity = newQuantity;
                    item.PriceInUsd = req.Price; // Update to latest price

                    await cartService.UpdatePartialCartAsync(currentUser.Id, $"$.items['{req.ProductId}']", item);
                    cart.Items[req.ProductId] = item;
                }
            }
            else
            {
                // Item doesn't exist - Add to dictionary (only if quantity > 0)
                if (req.Quantity > 0)
                {
                    item = new ShoppingCartItem
                    {
                        ProductId = req.ProductId,
                        ProductName = req.ProductName,
                        PriceInUsd = req.Price,
                        Quantity = req.Quantity,
                        ImageUrl = req.ImageUrl
                    };

                    await cartService.UpdatePartialCartAsync(currentUser.Id, $"$.items['{req.ProductId}']", item);
                    cart.Items[req.ProductId] = item;
                }
            }

            // Always update the timestamp and total price
            await cartService.UpdatePartialCartAsync(currentUser.Id, "$.updatedAtUtc", DateTime.UtcNow);
            await cartService.UpdatePartialCartAsync(currentUser.Id, "$.totalPriceInUsd", cart.TotalPriceInUsd);
        }

        return new Response(currentUser.Id);
    }
}
