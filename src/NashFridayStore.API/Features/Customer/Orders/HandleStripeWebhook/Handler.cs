using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NashFridayStore.Domain.Entities.Carts;
using NashFridayStore.Domain.Entities.Orders;
using NashFridayStore.Infrastructure.AppOptions;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace NashFridayStore.API.Features.Customer.Orders.HandleStripeWebhook;

public sealed class Handler(
    StoreDbContext dbContext,
    ICartService cartService,
    IOptions<StripeOptions> stripeOptions,
    IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request request, CancellationToken ct)
    {
        // Validation to check if stripe-header presences
        ValidationResult validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // verify stripe signature
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                request.JsonPayload,
                request.StripeSignature,
                stripeOptions.Value.WebhookSecret
            );
        }
        catch (StripeException)
        {
            throw new Exceptions.InvalidSignatureException();
        }

        // only catch the checkout session completed
        switch (stripeEvent.Type)
        {
            case EventTypes.CheckoutSessionCompleted:
                var session = stripeEvent.Data.Object as Session;
                await HandleCheckoutSessionCompletedAsync(session!, ct);
                break;
        }

        return new Response();
    }

    private async Task HandleCheckoutSessionCompletedAsync(Session session, CancellationToken ct)
    {
        bool orderExists = await dbContext.Orders
            .AnyAsync(x => x.StripeCheckoutSessionId == session.Id, ct);

        // If order already exists, skip, no need to create new order
        if (orderExists)
        {
            return;
        }

        // Get infor from metadata that we pass from the /api/orders/checkout endpoint
        var customerId = Guid.Parse(session.Metadata["CustomerId"]);
        string customerName = session.Metadata["CustomerName"];
        string customerEmail = session.Metadata["CustomerEmail"];
        string deliveryAddress = session.Metadata["DeliveryAddress"];
        string phoneNumber = session.Metadata["PhoneNumber"];

        // Get cart items from Redis
        ShoppingCart? cart = await cartService.GetCartAsync<ShoppingCart>(customerId);
        if (cart == null || !cart.Items.Any())
        {
            return;
        }

        // Create the order object
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            CustomerFullName = customerName,
            CustomerEmail = customerEmail,
            DeliveryAddress = deliveryAddress,
            PhoneNumber = phoneNumber,
            Currency = session.Currency.ToUpper(),
            TotalPriceInUsd = cart.TotalPriceInUsd,
            StripeCheckoutSessionId = session.Id,
            StripePaymentIntentId = session.PaymentIntentId,
            OrderStatus = OrderStatus.Completed,
            PaymentStatus = PaymentStatus.Paid,
            CreatedAtUtc = DateTime.UtcNow,
            OrderItems = cart.Items.Select(item => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.Value.ProductId,
                ProductName = item.Value.ProductName,
                CategoryId = item.Value.CategoryId,
                CategoryName = item.Value.CategoryName,
                Quantity = item.Value.Quantity,
                ProductUnitPriceInUsd = item.Value.PriceInUsd
            }).ToList()
        };

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(ct);
        await cartService.DeleteCartAsync(customerId);
    }
}
