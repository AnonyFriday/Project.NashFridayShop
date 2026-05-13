namespace NashFridayStore.API.Features.Customer.Orders.HandleStripeWebhook;

public sealed record Request(string JsonPayload, string? StripeSignature);
