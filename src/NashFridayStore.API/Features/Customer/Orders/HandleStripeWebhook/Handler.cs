using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NashFridayStore.Domain.Entities.Orders;
using NashFridayStore.Infrastructure.AppOptions;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.Interfaces.Payment;
using Stripe;
using Stripe.Checkout;

namespace NashFridayStore.API.Features.Customer.Orders.HandleStripeWebhook;

public sealed class Handler(
    StoreDbContext dbContext,
    ICheckoutService checkoutService,
    IOptions<StripeOptions> stripeOptions,
    IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request request, CancellationToken ct)
    {
        // Validation to check if stripe-header presence
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

        switch (stripeEvent.Type)
        {
            case EventTypes.CheckoutSessionCompleted:
                if (stripeEvent.Data.Object is Session session)
                {
                    await HandleCheckoutSessionCompletedAsync(session, ct);
                }
                break;

            case EventTypes.PaymentIntentPaymentFailed:
                if (stripeEvent.Data.Object is PaymentIntent failedIntent)
                {
                    await HandlePaymentIntentFailedAsync(failedIntent, ct);
                }
                break;

            case EventTypes.ChargeRefunded:
                if (stripeEvent.Data.Object is Charge charge)
                {
                    await HandleChargeRefundedAsync(charge, ct);
                }
                break;
        }

        return new Response();
    }

    private async Task HandleCheckoutSessionCompletedAsync(Session session, CancellationToken ct)
    {
        var command = new PaymentReceivedCommand(
            CustomerId: Guid.Parse(session.Metadata["CustomerId"]),
            CustomerName: session.Metadata["CustomerName"],
            CustomerEmail: session.Metadata["CustomerEmail"],
            DeliveryAddress: session.Metadata["DeliveryAddress"],
            PhoneNumber: session.Metadata["PhoneNumber"],
            Currency: session.Currency.ToUpper(),
            StripeCheckoutSessionId: session.Id,
            StripePaymentIntentId: session.PaymentIntentId);

        await checkoutService.HandlePaymentReceivedAsync(command, ct);
    }

    private async Task HandlePaymentIntentFailedAsync(PaymentIntent paymentIntent, CancellationToken ct)
    {
        Order? order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.StripePaymentIntentId == paymentIntent.Id, ct);

        if (order != null)
        {
            await checkoutService.HandlePaymentFailedAsync(
                new PaymentFailedCommand(order.Id, paymentIntent.Id), ct);
        }
    }

    private async Task HandleChargeRefundedAsync(Charge charge, CancellationToken ct)
    {
        Order? order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.StripePaymentIntentId == charge.PaymentIntentId, ct);

        if (order != null)
        {
            await checkoutService.HandleRefundAsync(
                new RefundCommand(order.Id, charge.PaymentIntentId), ct);
        }
    }
}
