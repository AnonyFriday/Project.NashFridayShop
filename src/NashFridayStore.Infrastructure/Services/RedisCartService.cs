using System.Text.Json;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.Interfaces;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace NashFridayStore.Infrastructure.Services;

public class RedisCartService(IConnectionMultiplexer connectionMultiplexer) : ICartService
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public async Task DeleteCartAsync(Guid customerId)
    {
        string key = BuildCartKey(customerId.ToString());

        await _database.KeyDeleteAsync(key);
    }

    public async Task<T?> GetCartAsync<T>(Guid customerId)
        where T : class
    {
        string key = BuildCartKey(customerId.ToString());

        return await _database.JSON().GetAsync<T>(key, serializerOptions: _jsonOptions);
    }

    public async Task SetCartAsync<T>(Guid customerId, T cart, TimeSpan? expiry = null)
        where T : class
    {
        if (cart is null)
        {
            return;
        }

        string key = BuildCartKey(customerId.ToString());

        await _database.JSON()
                .SetAsync(key, "$", cart, serializerOptions: _jsonOptions);

        // set TTL if having
        await _database.KeyExpireAsync(
            key,
            expiry ?? TimeSpan.FromMinutes(AppCts.Cart.CartimeToLiveInMinutes)
        );
    }

    public async Task UpdatePartialCartAsync<T>(Guid customerId, string path, T value)
    {
        if (value is null)
        {
            return;
        }

        string key = BuildCartKey(customerId.ToString());

        await _database.JSON()
            .SetAsync(key, path, value, serializerOptions: _jsonOptions);
    }

    public async Task DeletePartialCartAsync(Guid customerId, string path)
    {
        string key = BuildCartKey(customerId.ToString());

        await _database.JSON().DelAsync(key, path);
    }

    private static string BuildCartKey(string customerId)
    {
        return $"cart:{customerId}";
    }
}
