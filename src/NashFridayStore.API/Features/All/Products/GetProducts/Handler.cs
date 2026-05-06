using System.Collections.Immutable;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.API.Extensions;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.All.Products.GetProducts;

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
        IQueryable<Product> query = dbContext.Products;
        query = query
            .AsNoTracking()
            .Include(p => p.ProductRatings)
            .Where(x =>
                (!req.Status.HasValue || x.Status == req.Status.Value) &&
                (!req.CategoryId.HasValue || x.CategoryId == req.CategoryId.Value) &&
                (string.IsNullOrWhiteSpace(req.SearchName) || x.Name.Contains(req.SearchName)) &&
                (!req.MinPrice.HasValue || x.PriceUsd >= req.MinPrice.Value) &&
                (!req.MaxPrice.HasValue || x.PriceUsd <= req.MaxPrice.Value)
            );

        int totalItems = await query.CountAsync(ct);

        query = query
            .Skip(req.PageIndex * req.PageSize)
            .Take(req.PageSize);

        IEnumerable<ProductItem> items = await query
            .Select(x => new ProductItem(
                x.Id,
                x.Name,
                x.ImageUrl,
                x.PriceUsd,
                x.Status,
                (x.ProductRatings.Any() ? x.ProductRatings.Average(x => (decimal)x.Stars) % AppCts.Api.MaxStars : 0).NormalizeRating(),
                x.Quantity,
                DateTime.Compare(x.CreatedAtUtc, DateTime.UtcNow) < 7)
            ).ToListAsync(ct);

        // Sorting
        items = req.SortBy switch
        {
            SortBy.NameDesc => items.OrderByDescending(x => x.Name),
            SortBy.PriceDesc => items.OrderByDescending(x => x.PriceUsd),
            SortBy.RatingDesc => items.OrderByDescending(x => x.AverageStars),
            SortBy.NameAsc => items.OrderBy(x => x.Name),
            SortBy.PriceAsc => items.OrderBy(x => x.PriceUsd),
            SortBy.RatingAsc => items.OrderBy(x => x.AverageStars),
            _ => items.OrderBy(x => x.isNew),
        };

        int totalPages = (int)Math.Ceiling(totalItems / (double)req.PageSize);

        return new Response(
            items.ToImmutableArray(),
            totalItems,
            totalPages,
            req.PageIndex);
    }
}
