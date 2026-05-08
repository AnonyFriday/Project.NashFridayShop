namespace NashFridayStore.StoreFront.Services.Identity;

public interface IAccountApiClient
{
    Task<GetUserInfo.Response> GetUserInfoAsync();
    string GetLoginRedirectAddress(string? returnUrl);
    string GetLogoutRedirectAddress(string? returnUrl);
    string GetRegisterRedirectAddress(string? returnUrl);
}

