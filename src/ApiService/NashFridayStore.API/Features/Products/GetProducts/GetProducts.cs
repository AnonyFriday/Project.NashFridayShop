using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Products.GetProducts;

#region Contracts
public sealed record ProductItem(Guid Id, string Name, string ImageUrl, decimal PriceUsd);

public sealed record Request(
    Guid? CategoryId,
    string? SearchName,
    decimal? MinPrice,
    decimal? MaxPrice,
    int PageIndex = 0,
    int PageSize = 10,
    ProductStatus Status = ProductStatus.InStock,
    bool IsDeleted = false);

public sealed record Response(
    IReadOnlyList<ProductItem> ProductItems,
    int TotalItems,
    int TotalPages,
    int PageIndex);
#endregion

#region Validation
public sealed class Validator : AbstractValidator<Request>
{
    public const string SearchNameMaxLength = "Search has a maximum length of 100 characters.";
    public const string MinPriceLessThanOrEqualMaxPrice = "Min Price must be less than or equal to Max Price.";
    public const string PageIndexGreaterThanOrEqualZero = "Page must be greater than or equal to 0.";
    public const string PageSizeBetweenRange = "Page Size must be between 1 and 100.";
    public const string InvalidProductStatus = "Invalid product status.";
    public const string MinPriceGreaterThanOrEqualZero = "Min Price must be greater than or equal to 0.";
    public const string MaxPriceGreaterThanOrEqualZero = "Max Price must be greater than or equal to 0.";

    public Validator()
    {
        RuleFor(x => x.SearchName)
            .MaximumLength(100)
            .WithMessage(SearchNameMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchName));

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MinPriceGreaterThanOrEqualZero)
            .When(x => x.MinPrice.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MaxPriceGreaterThanOrEqualZero)
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x)
            .Must(x => x.MinPrice <= x.MaxPrice)
            .WithMessage(MinPriceLessThanOrEqualMaxPrice)
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage(PageIndexGreaterThanOrEqualZero);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(PageSizeBetweenRange);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(InvalidProductStatus);
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
            PageSize = orgReq.PageSize < 0 ? AppCts.Api.PageSize : orgReq.PageSize,
            SearchName = string.IsNullOrWhiteSpace(orgReq.SearchName) ? null : orgReq.SearchName.Trim(),
            MinPrice = orgReq.MinPrice is < 0 ? null : orgReq.MinPrice,
            MaxPrice = orgReq.MaxPrice is < 0 ? null : orgReq.MaxPrice
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw GetProductsErrors.Validation(validation.Errors);
        }

        // Handle Exceptions
        if (req.CategoryId.HasValue && !await dbContext.Categories
                .AnyAsync(x => x.Id == req.CategoryId.Value, ct))
        {
            throw GetProductsErrors.CategoryNotFound(req.CategoryId.Value);
        }

        // Implementing logic
        IQueryable<Product> query = dbContext.Products.AsQueryable();

        query = query
            .AsNoTracking()
            .Where(x =>
                x.IsDeleted == req.IsDeleted &&
                x.Status == req.Status &&
                (!req.CategoryId.HasValue || x.CategoryId == req.CategoryId.Value) &&
                (string.IsNullOrWhiteSpace(req.SearchName) || x.Name.Contains(req.SearchName)) &&
                (!req.MinPrice.HasValue || x.PriceUsd >= req.MinPrice.Value) &&
                (!req.MaxPrice.HasValue || x.PriceUsd <= req.MaxPrice.Value)
            );

        int totalItems = await query.CountAsync(ct);

        List<ProductItem> items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip(req.PageIndex * req.PageSize)
            .Take(req.PageSize)
            .Select(x => new ProductItem(
                x.Id,
                x.Name,
                x.ImageUrl,
                x.PriceUsd)
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
#endregion

#region Endpoints
[ApiController]
[Route("api/products")]
public class GetProductsController(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Request request,
        CancellationToken ct
    )
    {
        Response response = await handler.Handle(request, ct);
        return Ok(response);
    }
}
#endregion
