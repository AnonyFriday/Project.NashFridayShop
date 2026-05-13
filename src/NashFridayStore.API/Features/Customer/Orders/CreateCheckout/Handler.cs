using System.Collections.Immutable;
using FluentValidation;
using FluentValidation.Results;
using NashFridayStore.API.Auth;
using NashFridayStore.Domain.Entities.Carts;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.Interfaces;
using NashFridayStore.Infrastructure.Interfaces.Payment;

namespace NashFridayStore.API.Features.Customer.Orders.CreateCheckout;

public sealed class Handler(
    ICartService cartService,
    StoreDbContext dbContext,
    ICheckoutService checkoutService,
    ICurrentUser currentUser,
    IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
    {
        // Validation
        ValidationResult validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // If cart is empty
        ShoppingCart? cart = await cartService.GetCartAsync<ShoppingCart>(currentUser.Id);

        if (cart == null || !cart.Items.Any())
        {
            throw new Exceptions.CartIsEmptyException();
        }

        // Left Join Cart for better performance then querying in for loop
        var products = cart.Items
                .LeftJoin(dbContext.Products, c => c.Key, p => p.Id, (c, p) => new { productCart = c, productDB = p })
                .Select(x => new
                {
                    Id = x.productCart.Key,
                    NameInCart = x.productCart.Value.ProductName,
                    PriceInCart = x.productCart.Value.PriceInUsd,
                    QuantityInCart = x.productCart.Value.Quantity,
                    CategoryId = x.productCart.Value.CategoryId,
                    CategoryName = x.productCart.Value.CategoryName,
                    NameInDb = x.productDB?.Name,
                    DescriptionInDb = x.productDB?.Description,
                    ImageInDb = x.productDB?.ImageUrl,
                    PriceInDb = x.productDB?.PriceUsd,
                    QuantityInDb = x.productDB?.Quantity,
                    StatusInDb = x.productDB?.Status,
                    IsDeletedInDb = x.productDB?.IsDeleted
                })
                .ToImmutableList();

        foreach (var product in products)
        {
            // Product has been disabled or not found
            if (product.IsDeletedInDb == null || product.IsDeletedInDb == true)
            {
                throw new Exceptions.ProductNotFoundException(product.Id);
            }

            // Product Quantity is not enough
            if (product.QuantityInDb == null || product.QuantityInDb < product.QuantityInCart)
            {
                throw new Exceptions.ProductNotEnoughStockException(product.NameInCart, product.QuantityInCart, product.QuantityInDb ?? 0);
            }

            // Product current out of stock
            if (product.StatusInDb == ProductStatus.OutOfStock)
            {
                throw new Exceptions.ProductOutOfStockException(product.NameInCart);
            }

            // Product no longer available for purchase
            if (product.StatusInDb == ProductStatus.Discontinued)
            {
                throw new Exceptions.ProductIsDiscontinuedException(product.NameInCart);
            }

            // if the price or name of the product changed, throw exception to user to update a cart
            if (product.NameInDb != product.NameInCart || product.PriceInDb != product.PriceInCart)
            {
                throw new Exceptions.ProductPriceOrNameChangedException(product.NameInCart);
            }
        }

        // Create a checkout session
        IEnumerable<ICheckoutService.CheckoutItemDto> checkoutItems = products.Select(p => new ICheckoutService.CheckoutItemDto(
            p.Id,
            p.NameInCart,
            p.DescriptionInDb,
            p.ImageInDb,
            p.PriceInCart,
            p.QuantityInCart,
            p.CategoryId,
            p.CategoryName));

        var customer = new ICheckoutService.CustomerCheckoutDto(
            currentUser.Id,
            request.CustomerName,
            request.CustomerEmail,
            request.DeliveryAddress,
            request.PhoneNumber);

        string checkoutUrl = await checkoutService.CreateCheckoutSessionAsync(checkoutItems, customer, cancellationToken);
        return new Response(checkoutUrl);
    }
}
