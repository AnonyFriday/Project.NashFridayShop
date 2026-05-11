using System.Globalization;
using Microsoft.Extensions.Options;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.AppOptions;
using NashFridayStore.Infrastructure.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace NashFridayStore.Infrastructure.Services;

public class StripeCheckoutService(
    StripeClient stripeClient,
    IOptions<StripeOptions> stripeOptions) : ICheckoutService
{
    public async Task<string> CreateCheckoutSessionAsync(
        IEnumerable<ICheckoutService.CheckoutItemDto> items,
        ICheckoutService.CustomerCheckoutDto customer,
        CancellationToken cancellationToken = default)
    {
        var lineItems = items.Select(p => new SessionLineItemOptions
        {
            Quantity = p.Quantity,
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = AppCts.Currency.Usd,
                UnitAmount = (long)(p.PriceInUsd * 100), // stripe expect in cents
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = p.Name,
                    Description = p.Description,
                    Images = p.ImageUrl != null ? [p.ImageUrl] : null,
                    Metadata = new Dictionary<string, string>
                    {
                        ["CategoryId"] = p.CategoryId.ToString(),
                        ["CategoryName"] = p.CategoryName
                    }
                }
            }
        }).ToList();

        var metadata = new Dictionary<string, string>
        {
            ["CustomerId"] = customer.Id.ToString(),
            ["CustomerName"] = customer.Name,
            ["CustomerEmail"] = customer.Email,
            ["DeliveryAddress"] = customer.DeliveryAddress,
            ["PhoneNumber"] = customer.PhoneNumber
        };

        var sessionOptions = new SessionCreateOptions
        {
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = stripeOptions.Value.ReturnSuccessUrl,
            CancelUrl = stripeOptions.Value.ReturnCancelUrl,
            CustomerEmail = customer.Email,
            Metadata = metadata
        };

        var service = new SessionService(stripeClient);
        Session session = await service.CreateAsync(sessionOptions, cancellationToken: cancellationToken);
        return session.Url;
    }
}
