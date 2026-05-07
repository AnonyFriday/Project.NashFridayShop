namespace NashFridayStore.StoreFront.Services.Identity;

public class IdentityApiClient(BaseApiClient apiClient) : IIdentityApiClient
{
    public async Task<GetUserInfo.Response> GetUserInfoAsync()
    {
        GetUserInfo.Response? response = await apiClient.GetAsync<GetUserInfo.Response>("auth/me");

        if (response == null || !response.IsAuthenticated)
        {
            return new GetUserInfo.Response(false, null!, null!, null!);
        }

        return response;
    }
}
