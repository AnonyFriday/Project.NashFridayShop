using System.Text.Json;
using System.Text.Json.Serialization;

namespace NashFridayStore.StoreFront.Services;

public class BaseApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<T>(endpoint, _jsonOptions);
        }
        catch (Exception ex)
        {
            TriggerToast($"Error fetching data: {ex.Message}", "error");
            return default;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, request, _jsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                TriggerToast($"Request failed: {response.ReasonPhrase}", "warning");
                return default;
            }

            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }
        catch (Exception ex)
        {
            TriggerToast($"Error sending data: {ex.Message}", "error");
            return default;
        }
    }

    public async Task<bool> PutAsync<TRequest>(string endpoint, TRequest request)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(endpoint, request, _jsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                TriggerToast($"Update failed: {response.ReasonPhrase}", "warning");
            }
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            TriggerToast($"Error updating data: {ex.Message}", "error");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                TriggerToast($"Delete failed: {response.ReasonPhrase}", "warning");
            }
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            TriggerToast($"Error deleting data: {ex.Message}", "error");
            return false;
        }
    }

    private void TriggerToast(string message, string type)
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return;
        }

        var toastEvent = new { message, type };
        // Using Dictionary to support dash in JSON key
        var trigger = new Dictionary<string, object>
        {
            { "show-toast", toastEvent }
        };
        string triggerJson = JsonSerializer.Serialize(trigger);

        // trigger show-toast with message and type as data
        httpContext.Response.Headers.Append("HX-Trigger", triggerJson);
    }
}
