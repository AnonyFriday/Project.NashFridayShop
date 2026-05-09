using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.Interfaces;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace NashFridayStore.Infrastructure.Services;

public class RedisCartService(IConnectionMultiplexer connectionMultiplexer) : ICartService
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    public async Task DeleteCartAsync(string customerId)
    {
        string key = BuildCartKey(customerId);

        await _database.KeyDeleteAsync(key);
    }

    public async Task<T?> GetCartAsync<T>(string customerId)
        where T : class
    {
        string key = BuildCartKey(customerId);

        return await _database.JSON().GetAsync<T>(key);
    }

    public async Task SetCartAsync<T>(string customerId, T cart, TimeSpan? expiry = null)
        where T : class
    {
        if (cart is null)
        {
            return;
        }

        string key = BuildCartKey(customerId);

        await _database.JSON()
                .SetAsync(key, "$", cart);

        // set TTL if having
        if (expiry.HasValue)
        {
            await _database.KeyExpireAsync(
                key,
                expiry.Value
            );
        }
    }

    public async Task UpdatePartialCartAsync<T>(string customerId, string path, T value)
    {
        if (value is null)
        {
            return;
        }

        string key = BuildCartKey(customerId);

        await _database.JSON()
            .SetAsync(key, path, value);
    }

    private static string BuildCartKey(string customerId)
    {
        return $"cart:{customerId}";
    }
}
