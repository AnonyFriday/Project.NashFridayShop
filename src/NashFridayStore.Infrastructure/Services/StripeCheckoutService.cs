using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities.Carts;
using NashFridayStore.Domain.Entities.Orders;
using NashFridayStore.Infrastructure.AppOptions;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.Interfaces;
using NashFridayStore.Infrastructure.Interfaces.Payment;
using Stripe;
using Stripe.Checkout;

namespace NashFridayStore.Infrastructure.Services;

public class StripeCheckoutService(
    StripeClient stripeClient,
    StoreDbContext dbContext,
    ICartService cartService,
    ILogger<StripeCheckoutService> logger,
    IOptions<StripeOptions> stripeOptions) : ICheckoutService
{
    public async Task<string> CreateCheckoutSessionAsync(
        IEnumerable<ICheckoutService.CheckoutItemDto> items,
        ICheckoutService.CustomerCheckoutDto customer,
        CancellationToken ct)
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
            Metadata = metadata,
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                Metadata = metadata
            }
        };

        var service = new SessionService(stripeClient);
        Session session = await service.CreateAsync(sessionOptions, cancellationToken: ct);
        return session.Url;
    }

    public async Task HandlePaymentFailedAsync(PaymentFailedCommand command, CancellationToken ct)
    {
        Order? order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == command.OrderId, ct);
        if (order is null)
        {
            return;
        }

        order.PaymentStatus = PaymentStatus.Failed;
        order.OrderStatus = OrderStatus.Cancelled;
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task HandlePaymentReceivedAsync(PaymentReceivedCommand command, CancellationToken ct)
    {
        bool orderExists = await dbContext.Orders.AnyAsync(
              x => x.StripeCheckoutSessionId == command.StripeCheckoutSessionId
                  || x.StripePaymentIntentId == command.StripePaymentIntentId,
              ct);

        // if exists, then just ignore, no need to create new order
        if (orderExists)
        {
            return;
        }

        ShoppingCart? cart = await cartService.GetCartAsync<ShoppingCart>(command.CustomerId);

        // check on cart
        if (cart is null || !cart.Items.Any())
        {
            return;
        }

        // Create a transaction here, recheck again
        // Another customer maybe at the current payment filling just like us, so be sure to handle the quantity of the product
        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(ct);
        try
        {
            var productIds = cart.Items.Keys.ToList();
            Dictionary<Guid, Domain.Entities.Products.Product> products =
                await dbContext.Products
                    .Where(x => productIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        product = x
                    })
                    .ToDictionaryAsync(
                        x => x.Id,
                        x => x.product,
                        ct);

            // Check quantity
            foreach (ShoppingCartItem item in cart.Items.Values)
            {
                int quantityInDb = products[item.ProductId].Quantity;
                if (quantityInDb < item.Quantity)
                {
                    logger.LogWarning("Product {ProductName} is out of stock, current quantity: {CurrentQuantity}", item.ProductName, quantityInDb);
                    // throw or rollback
                    await transaction.RollbackAsync(ct);
                    throw new Exception($"Product {item.ProductName} is out of stock, current quantity: {quantityInDb}");
                }
            }

            // deduct stock
            foreach (ShoppingCartItem item in cart.Items.Values)
            {
                products[item.ProductId].Quantity -= item.Quantity;
            }

            // Create the order object
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = command.CustomerId,
                CustomerFullName = command.CustomerName,
                CustomerEmail = command.CustomerEmail,
                DeliveryAddress = command.DeliveryAddress,
                PhoneNumber = command.PhoneNumber,
                Currency = command.Currency.ToUpper(),
                TotalPriceInUsd = cart.TotalPriceInUsd,
                StripeCheckoutSessionId = command.StripeCheckoutSessionId,
                StripePaymentIntentId = command.StripePaymentIntentId,
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
            await transaction.CommitAsync(ct); // if saving to db successfull, then delete a cart to avoid failling the information
            await cartService.DeleteCartAsync(command.CustomerId);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task HandleRefundAsync(RefundCommand command, CancellationToken ct)
    {
        Order? order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == command.OrderId, ct);
        if (order is null)
        {
            return;
        }

        // I dont add quantity back, cuz in the real life, once the package has been sent, then the stock is 
        // refilled back to the inventory

        order.PaymentStatus = PaymentStatus.Refunded;
        order.OrderStatus = OrderStatus.Refunded;
        await dbContext.SaveChangesAsync(ct);
    }
}
