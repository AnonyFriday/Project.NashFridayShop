namespace NashFridayStore.StoreFront.Services.Identity;

public static class GetUserInfo
{
    public record Response(Guid CustomerId, bool IsAuthenticated, string? Email, string? PhoneNumber, List<string>? Roles);
}
