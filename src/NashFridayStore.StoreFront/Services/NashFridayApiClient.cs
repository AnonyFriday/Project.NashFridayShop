namespace NashFridayStore.StoreFront.Services;

public class BaseApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    // === Products ===
    // === Categories ===
    // === Cart ===
    // === Auth ===
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        return await _httpClient.GetFromJsonAsync<T>(endpoint);
    }
}
