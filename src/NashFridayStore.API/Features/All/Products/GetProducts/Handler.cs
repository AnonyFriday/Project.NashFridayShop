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
            PageSize = orgReq.PageSize <= 0 ? AppCts.Api.PageSize : orgReq.PageSize,
            SearchName = string.IsNullOrWhiteSpace(orgReq.SearchName)
                ? null
                : orgReq.SearchName.Trim(),
            MinPrice = orgReq.MinPrice is < 0 ? null : orgReq.MinPrice,
            MaxPrice = orgReq.MaxPrice is < 0 ? null : orgReq.MaxPrice
        };

        // Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);

        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Implement Logic
        IQueryable<Product> query = dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductRatings)
            .Where(x =>
                (!req.Status.HasValue || x.Status == req.Status.Value) &&
                (!req.CategoryId.HasValue || x.CategoryId == req.CategoryId.Value) &&
                (string.IsNullOrWhiteSpace(req.SearchName) ||
                 x.Name.Contains(req.SearchName)) &&
                (!req.MinPrice.HasValue || x.PriceUsd >= req.MinPrice.Value) &&
                (!req.MaxPrice.HasValue || x.PriceUsd <= req.MaxPrice.Value)
            );

        // Total Items
        int totalItems = await query.CountAsync(ct);

        // Sorting
        query = req.SortBy switch
        {
            SortBy.NameAsc => query.OrderBy(x => x.Name),
            SortBy.NameDesc => query.OrderByDescending(x => x.Name),
            SortBy.PriceAsc => query.OrderBy(x => x.PriceUsd),
            SortBy.PriceDesc => query.OrderByDescending(x => x.PriceUsd),
            SortBy.RatingAsc => query.OrderBy(x => x.ProductRatings.Any()
                        ? x.ProductRatings.Average(r => r.Stars)
                        : 0),
            SortBy.RatingDesc => query.OrderByDescending(x => x.ProductRatings.Any()
                        ? x.ProductRatings.Average(r => r.Stars)
                        : 0),
            _ => query.OrderByDescending(x => x.CreatedAtUtc)
        };

        // Pagination
        IEnumerable<ProductItem> items = await query
            .Skip(req.PageIndex * req.PageSize)
            .Take(req.PageSize)
            .LeftJoin(
                dbContext.Categories,
                p => p.CategoryId,
                c => c.Id,
                (p, c) => new
                {
                    Product = p,
                    CategoryName = c != null ? c.Name : "Others"
                }
            )
            .Select(x => new ProductItem(
                x.Product.Id,
                x.Product.Name,
                x.CategoryName,
                x.Product.ImageUrl,
                x.Product.PriceUsd,
                x.Product.Status,
                (x.Product.ProductRatings.Any() ? x.Product.ProductRatings.Average(r => (decimal)r.Stars) : 0).NormalizeRating(),
                x.Product.Quantity,
                (DateTime.UtcNow - x.Product.CreatedAtUtc).TotalDays <= 7
            ))
            .ToListAsync(ct);

        // Total Pages
        int totalPages = (int)Math.Ceiling(
            totalItems / (double)req.PageSize);

        return new Response(
            items.ToImmutableArray(),
            totalItems,
            totalPages,
            req.PageIndex);
    }
}
