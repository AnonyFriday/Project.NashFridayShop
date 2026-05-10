namespace NashFridayStore.Infrastructure.Interfaces;

public interface ICartService
{
    Task SetCartAsync<T>(Guid customerId, T cart, TimeSpan? expiry = null) where T : class;
    Task<T?> GetCartAsync<T>(Guid customerId) where T : class;
    Task DeleteCartAsync(Guid customerId);
    Task UpdatePartialCartAsync<T>(Guid customerId, string path, T value);
    Task DeletePartialCartAsync(Guid customerId, string path);
}
