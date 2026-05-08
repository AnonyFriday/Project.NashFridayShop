using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.API.Extensions;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.All.Products.GetProduct;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        Response? product = await dbContext.Products
            .AsNoTracking()
            .Include(p => p.ProductRatings)
            .Where(x => x.Id == req.Id)
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
            .Select(x => new Response(
                x.Product.Id,
                x.Product.CategoryId,
                x.CategoryName,
                x.Product.Name,
                x.Product.Description,
                x.Product.ImageUrl,
                x.Product.PriceUsd,
                x.Product.Quantity,
                x.Product.Status,
                (x.Product.ProductRatings.Any() ? x.Product.ProductRatings.Average(r => (decimal)r.Stars) : 0).NormalizeRating()))
            .FirstOrDefaultAsync(ct);

        if (product is null)
        {
            throw new Exceptions.ProductNotFoundException(req.Id);
        }

        return product;
    }
}
