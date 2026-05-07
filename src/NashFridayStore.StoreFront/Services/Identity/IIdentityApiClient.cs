namespace NashFridayStore.StoreFront.Services.Identity;

public interface IIdentityApiClient
{
    Task<GetUserInfo.Response> GetUserInfoAsync();
}

