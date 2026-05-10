using System.Net.Http.Json;
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

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, request, _jsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
    }

    public async Task<bool> PutAsync<TRequest>(string endpoint, TRequest request)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(endpoint, request, _jsonOptions);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }
}
