using Microsoft.AspNetCore.Mvc;
using NashFridayStore.StoreFront.Services.Identity;

namespace NashFridayStore.StoreFront.Pages.Profile;

public class IndexModel(IAccountApiClient accountApiClient) : BasePageModel
{
    public GetUserInfo.Response Profile { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        Profile = await accountApiClient.GetUserInfoAsync() ?? default!;

        if (Profile != null && Profile.IsAuthenticated)
        {
            return Page();
        }

        return RedirectToPage("/Index");
    }
}
