using System.Text.Json;
using System.Text.Json.Serialization;
using NashFridayStore.StoreFront.Commons.Exceptions;

namespace NashFridayStore.StoreFront.Services;

public class BaseApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        HttpResponseMessage response = await httpClient.GetAsync(endpoint);
        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(endpoint, request, _jsonOptions);
        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        // If the response is successful in range 200, return
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        // since we can only detect the exception from api server via error code, so at this point, we surely that api server return the exception
        // ApiErrorResponse follow the same format from the server
        ApiErrorResponse? error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(_jsonOptions);
        throw new ApiException(error ?? new ApiErrorResponse(
            "Unexpected error",
            "Unexpected error response format.",
            (int)response.StatusCode));
    }
}
