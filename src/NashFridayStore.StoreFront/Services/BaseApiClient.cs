using System.Text.Json;
using System.Text.Json.Serialization;

namespace NashFridayStore.StoreFront.Services;

public class BaseApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        return await _httpClient.GetFromJsonAsync<T>(endpoint, _jsonOptions);
    }
}
