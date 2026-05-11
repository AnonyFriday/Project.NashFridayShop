namespace NashFridayStore.Infrastructure.Interfaces;

public interface ICheckoutService
{
    public record CheckoutItemDto(
        Guid Id,
        string Name,
        string? Description,
        string? ImageUrl,
        decimal PriceInUsd,
        int Quantity);

    public record CustomerCheckoutDto(
        Guid Id,
        string Name,
        string Email,
        string DeliveryAddress,
        string PhoneNumber);

    Task<string> CreateCheckoutSessionAsync(
        IEnumerable<CheckoutItemDto> items,
        CustomerCheckoutDto customer,
        CancellationToken cancellationToken = default);
}
