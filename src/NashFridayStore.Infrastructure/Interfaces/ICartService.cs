namespace NashFridayStore.Infrastructure.Interfaces;

public interface ICartService
{
    Task SetCartAsync<T>(string customerId, T cart, TimeSpan? expiry = null) where T : class;
    Task<T?> GetCartAsync<T>(string customerId) where T : class;
    Task DeleteCartAsync(string customerId);
    Task UpdatePartialCartAsync<T>(string customerId, string path, T value);
}
