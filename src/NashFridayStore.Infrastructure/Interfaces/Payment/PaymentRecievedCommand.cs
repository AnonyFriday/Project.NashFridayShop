namespace NashFridayStore.Infrastructure.Interfaces.Payment;

public sealed record PaymentReceivedCommand(
    Guid CustomerId,
    string CustomerName,
    string CustomerEmail,
    string DeliveryAddress,
    string PhoneNumber,
    string Currency,
    string StripeCheckoutSessionId,
    string StripePaymentIntentId);
