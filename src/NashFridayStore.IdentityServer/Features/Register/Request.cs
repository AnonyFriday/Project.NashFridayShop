namespace NashFridayStore.IdentityServer.Features.Register;

public sealed record Request
{
    public string Email { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string ConfirmPassword { get; init; } = default!;
    public string? PhoneNumber { get; init; }
}
