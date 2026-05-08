using Microsoft.Extensions.Options;
using NashFridayStore.StoreFront.AppOptions;

namespace NashFridayStore.StoreFront.Services.Identity;

public class AccountApiClient(BaseApiClient apiClient, IOptions<ApiUrlOptions> options) : IAccountApiClient
{
    public string GetLoginRedirectAddress(string? returnUrl)
    {
        return $"{options.Value.RedirectedLoginUrl}?returnUrl={options.Value.StorefrontUrl}{returnUrl}";
    }

    public string GetLogoutRedirectAddress(string? returnUrl)
    {
        return $"{options.Value.RedirectedLogoutUrl}?returnUrl={options.Value.StorefrontUrl}{returnUrl}";
    }

    public string GetRegisterRedirectAddress(string? returnUrl)
    {
        return $"{options.Value.RedirectedRegisterUrl}?returnUrl={options.Value.StorefrontUrl}{returnUrl}";
    }

    public async Task<GetUserInfo.Response> GetUserInfoAsync()
    {
        GetUserInfo.Response response = await apiClient.GetAsync<GetUserInfo.Response>("api/auth/me");

        if (response == null || !response.IsAuthenticated)
        {
            return new GetUserInfo.Response(null, false, null, null, null);
        }

        return response;
    }
}
