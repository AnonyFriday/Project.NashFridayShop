namespace NashFridayStore.Infrastructure.Interfaces.Payment;

public sealed record PaymentFailedCommand(
    Guid OrderId,
    string StripePaymentIntentId
);

