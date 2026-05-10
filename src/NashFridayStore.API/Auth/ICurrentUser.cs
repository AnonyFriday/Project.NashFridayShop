namespace NashFridayStore.API.Auth;

public interface ICurrentUser
{
    Guid Id { get; }
    string? Name { get; }
    bool IsAuthenticated { get; }
}
