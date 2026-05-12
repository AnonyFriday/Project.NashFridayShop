using Microsoft.EntityFrameworkCore;
using NashFridayStore.API.Auth;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Customer.Orders.GetMyOrders;

public sealed class Handler(
    StoreDbContext dbContext,
    ICurrentUser currentUser)
{
    public async Task<Response> HandleAsync(CancellationToken ct)
    {
        List<OrderItemResponse> orders = await dbContext.Orders
            .AsNoTracking()
            .Where(x => x.CustomerId == currentUser.Id)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(o => new OrderItemResponse(
                o.Id,
                o.CustomerFullName,
                o.CustomerEmail,
                o.DeliveryAddress,
                o.PhoneNumber,
                o.Currency,
                o.TotalPriceInUsd,
                o.OrderStatus,
                o.PaymentStatus,
                o.CreatedAtUtc,
                o.OrderItems.Select(oi => new OrderItemDetailResponse(
                    oi.ProductId,
                    oi.ProductName,
                    oi.CategoryId,
                    oi.CategoryName,
                    oi.Quantity,
                    oi.ProductUnitPriceInUsd
                )).ToList()
            ))
            .ToListAsync(ct);

        return new Response(orders);
    }
}
