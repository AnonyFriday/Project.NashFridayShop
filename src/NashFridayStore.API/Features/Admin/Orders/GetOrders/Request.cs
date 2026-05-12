using NashFridayStore.Domain.Entities.Orders;

namespace NashFridayStore.API.Features.Admin.Orders.GetOrders;

public sealed record Request(
    int PageIndex = 0,
    int PageSize = 10,
    OrderStatus? OrderStatus = null,
    PaymentStatus? PaymentStatus = null);
