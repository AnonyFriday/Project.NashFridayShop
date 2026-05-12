using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities.Orders;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Admin.Orders.GetOrders;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        Request req = orgReq with
        {
            PageIndex = orgReq.PageIndex <= 0 ? AppCts.Api.PageIndex : orgReq.PageIndex,
            PageSize = orgReq.PageSize <= 0 ? AppCts.Api.PageSize : orgReq.PageSize
        };

        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        IQueryable<Order> query = dbContext.Orders.AsNoTracking();

        query = query.Where(x =>
            (!req.OrderStatus.HasValue || x.OrderStatus == req.OrderStatus.Value) &&
            (!req.PaymentStatus.HasValue || x.PaymentStatus == req.PaymentStatus.Value)
        );

        int totalItems = await query.CountAsync(ct);

        List<OrderItem> items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip(req.PageIndex * req.PageSize)
            .Take(req.PageSize)
            .Select(x => new OrderItem(
                x.Id,
                x.CustomerFullName,
                x.CustomerEmail,
                x.DeliveryAddress,
                x.PhoneNumber,
                x.Currency,
                x.TotalPriceInUsd,
                x.OrderStatus,
                x.PaymentStatus,
                x.CreatedAtUtc)
            )
            .ToListAsync(ct);

        int totalPages = (int)Math.Ceiling(totalItems / (double)req.PageSize);

        return new Response(
            items,
            totalItems,
            totalPages,
            req.PageIndex);
    }
}
