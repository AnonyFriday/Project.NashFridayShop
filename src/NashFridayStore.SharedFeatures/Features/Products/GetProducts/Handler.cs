using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.SharedFeatures.Features.Products.GetProducts;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
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
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Implementing logic
        var query = dbContext.Products.AsQueryable();

        query = query
            .AsNoTracking()
            .Include(p => p.ProductRatings)
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
                x.PriceUsd,
                x.Status,
                x.ProductRatings.Any() ? x.ProductRatings.Average(x => (decimal)x.Stars) % AppCts.Api.MaxStars : 0)
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
