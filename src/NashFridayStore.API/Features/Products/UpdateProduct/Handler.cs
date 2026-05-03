using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Products.UpdateProduct;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Clean request
        Request req = orgReq with
        {
            RequestBody = orgReq.RequestBody with
            {
                Name = orgReq.RequestBody.Name.Trim(),
                Description = orgReq.RequestBody.Description.Trim(),
            }
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Check if category exists
        bool categoryExists = await dbContext.Categories
            .AsNoTracking()
            .AnyAsync(x => x.Id == req.RequestBody.CategoryId, ct);

        if (!categoryExists)
        {
            throw new Exceptions.CategoryNotFoundException(req.RequestBody.CategoryId);
        }

        // Get product 
        IQueryable<Product> query = dbContext.Products.AsQueryable();
        if (req.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        Product? product = await query
            .FirstOrDefaultAsync(x => x.Id == req.ProductId, cancellationToken: ct);

        if (product == null)
        {
            throw new Exceptions.ProductNotFoundException(req.ProductId);
        }

        // Update product
        product.CategoryId = req.RequestBody.CategoryId;
        product.Name = req.RequestBody.Name;
        product.Description = req.RequestBody.Description;
        product.PriceUsd = req.RequestBody.PriceUsd;
        product.ImageUrl = req.RequestBody.ImageUrl;
        product.Quantity = req.RequestBody.Quantity;
        product.Status = req.RequestBody.Status;
        product.UpdatedAtUtc = DateTime.UtcNow;

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(ct);

        return new Response(
            product.Id,
            product.CategoryId,
            product.Name,
            product.Description,
            product.PriceUsd,
            product.ImageUrl,
            product.Quantity,
            product.Status,
            product.UpdatedAtUtc ?? DateTime.UtcNow);
    }
}
