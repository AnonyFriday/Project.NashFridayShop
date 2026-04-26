using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Products.CreateProduct;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
    {
        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Check if category exists
        bool categoryExists = await dbContext.Categories
            .AsNoTracking()
            .AnyAsync(x => x.Id == req.CategoryId, ct);

        if (!categoryExists)
        {
            throw new Exceptions.CategoryNotFoundException(req.CategoryId);
        }

        // Create new product
        var product = new Product
        {
            Id = Guid.NewGuid(),
            CategoryId = req.CategoryId,
            Name = req.Name,
            Description = req.Description,
            PriceUsd = req.PriceUsd,
            ImageUrl = req.ImageUrl,
            Quantity = req.Quantity,
            Status = req.Status,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        dbContext.Products.Add(product);
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
            product.CreatedAtUtc);
    }
}
