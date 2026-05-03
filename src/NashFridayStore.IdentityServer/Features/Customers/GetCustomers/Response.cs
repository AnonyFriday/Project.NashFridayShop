namespace NashFridayStore.IdentityServer.Features.Customers.GetCustomers;

public sealed record Response(
    List<CustomerItem> Items,
    int TotalItems,
    int TotalPages,
    int PageIndex);

public sealed record CustomerItem(
    Guid Id,
    string FullName,
    string UserName,
    string Email,
    string? Address,
    bool IsDeleted,
    DateTime CreatedAtUtc);
