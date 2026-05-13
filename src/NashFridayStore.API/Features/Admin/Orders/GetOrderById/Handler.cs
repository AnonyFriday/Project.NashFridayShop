using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities.Orders;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Admin.Orders.GetOrderById;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }
        Order? order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (order is null)
        {
            throw new Exceptions.OrderNotFoundException(req.Id);
        }

        return new Response(
            order.Id,
            order.CustomerFullName,
            order.CustomerEmail,
            order.DeliveryAddress,
            order.PhoneNumber,
            order.Currency,
            order.TotalPriceInUsd,
            order.OrderStatus,
            order.PaymentStatus,
            order.CreatedAtUtc,
            order.OrderItems.Select(x => new OrderItemDetail(
                x.ProductId,
                x.ProductName,
                x.CategoryId,
                x.CategoryName,
                x.Quantity,
                x.ProductUnitPriceInUsd)).ToList());
    }
}
