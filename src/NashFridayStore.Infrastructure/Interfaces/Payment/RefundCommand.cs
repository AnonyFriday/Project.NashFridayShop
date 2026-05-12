namespace NashFridayStore.Infrastructure.Interfaces.Payment;

public sealed record RefundCommand(
    Guid OrderId,
    string StripePaymentIntentId
);
