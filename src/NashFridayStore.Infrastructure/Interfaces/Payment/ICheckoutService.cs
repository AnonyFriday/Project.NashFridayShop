namespace NashFridayStore.Infrastructure.Interfaces.Payment;

public partial interface ICheckoutService
{
    public record CheckoutItemDto(
        Guid Id,
        string Name,
        string? Description,
        string? ImageUrl,
        decimal PriceInUsd,
        int Quantity,
        Guid CategoryId,
        string CategoryName);

    public record CustomerCheckoutDto(
        Guid Id,
        string Name,
        string Email,
        string DeliveryAddress,
        string PhoneNumber);

    Task<string> CreateCheckoutSessionAsync(
        IEnumerable<CheckoutItemDto> items,
        CustomerCheckoutDto customer,
        CancellationToken ct);

    Task HandlePaymentReceivedAsync(
        PaymentReceivedCommand command,
        CancellationToken ct);

    Task HandlePaymentFailedAsync(
        PaymentFailedCommand command,
        CancellationToken ct);

    Task HandleRefundAsync(
        RefundCommand command,
        CancellationToken ct);
}
