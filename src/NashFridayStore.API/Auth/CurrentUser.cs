namespace NashFridayStore.API.Auth;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid Id
    {
        get
        {
            // since we settings InBoundClaim to false, we already set "sub" in the claim, so get the name "sub" from here
            string? userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
            return Guid.TryParse(userIdClaim, out Guid guid) ? guid : Guid.Empty;
        }
    }

    public string? Name => httpContextAccessor.HttpContext?.User.Identity?.Name;

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
