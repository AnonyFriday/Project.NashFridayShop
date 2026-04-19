using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Products.GetProductRatings;

#region Contracts
public sealed record RatingItem(int Stars, string? Comment, DateTime CreatedAtUtc);

public sealed record Request(
    [property: System.Text.Json.Serialization.JsonRequired] Guid ProductId,
    int PageIndex = 0,
    int PageSize = 10,
    bool IsDeleted = false);

public sealed record Response(
    IReadOnlyList<RatingItem> Items,
    int TotalItems,
    int TotalPages,
    int PageIndex,
    decimal Average);
#endregion

#region Validation
public sealed class Validator : AbstractValidator<Request>
{
    public const string PageIndexGreaterThanOrEqualZero = "Page must be greater than or equal to 0.";
    public const string PageSizeBetweenRange = "Page Size must be between 1 and 100.";

    public Validator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product Id is required.");

        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageIndexGreaterThanOrEqualZero);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PageSizeBetweenRange);
    }
}
#endregion

#region Business Logic
public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> Handle(Request orgReq, CancellationToken ct)
    {
        // Cleaning Request
        Request req = orgReq with
        {
            PageIndex = orgReq.PageIndex <= 0 ? AppCts.Api.PageIndex : orgReq.PageIndex,
            PageSize = orgReq.PageSize < 0 ? AppCts.Api.PageSize : orgReq.PageSize
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw GetProductRatingsErrors.Validation(validation.Errors);
        }

        // Check if product exists
        bool productExists = await dbContext.Products
            .AsNoTracking()
            .AnyAsync(x => x.Id == req.ProductId && x.IsDeleted == req.IsDeleted, ct);

        if (!productExists)
        {
            throw GetProductRatingsErrors.ProductNotFound(req.ProductId);
        }

        // Get ratings
        IQueryable<ProductRating> query = dbContext.ProductRatings.AsQueryable();

        query = query
            .AsNoTracking()
            .Where(x =>
                x.ProductId == req.ProductId &&
                x.IsDeleted == req.IsDeleted);

        int totalItems = await query.CountAsync(ct);

        List<RatingItem> items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip(req.PageIndex * req.PageSize)
            .Take(req.PageSize)
            .Select(x => new RatingItem(
                x.Stars,
                x.Comment,
                x.CreatedAtUtc))
            .ToListAsync(ct);

        // Calculate average
        decimal average = await query.AnyAsync(ct)
            ? await query.AverageAsync(x => (decimal)x.Stars, ct)
            : 0;

        int totalPages = (int)Math.Ceiling(totalItems / (double)req.PageSize);

        return new Response(
            items,
            totalItems,
            totalPages,
            req.PageIndex,
            average);
    }
}
#endregion

#region Endpoints
[ApiController]
[Route("api/products/{productId:guid}/ratings")]
public class GetProductRatingsController(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid productId, [FromQuery] Request request, CancellationToken ct)
    {
        Request req = request with { ProductId = productId };
        Response response = await handler.Handle(req, ct);
        return Ok(response);
    }
}
#endregion
