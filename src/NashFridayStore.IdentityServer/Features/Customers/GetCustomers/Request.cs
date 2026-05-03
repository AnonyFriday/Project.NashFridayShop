namespace NashFridayStore.IdentityServer.Features.Customers.GetCustomers;

public sealed record Request(
    string? SearchName = null,
    int PageIndex = 0,
    int PageSize = 10,
    bool IncludeDeleted = false);
