using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Products.UpdateProductImage;

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

        IQueryable<Product> query = dbContext.Products.AsQueryable();

        if (req.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        // Get product 
        Product? product = await query
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == req.ProductId, cancellationToken: ct);

        if (product == null)
        {
            throw new Exceptions.ProductNotFoundException(req.ProductId);
        }

        // Handle File size
        if (req.ImageFile.Length > Exceptions.MaxImageSize)
        {
            throw new Exceptions.ImageTooLargeException(req.ImageFile.Length);
        }

        // Handle File Type
        string fileExtension = Path.GetExtension(req.ImageFile.FileName).ToLowerInvariant();
        if (!Exceptions.SupportedImageExtensions.Contains(fileExtension))
        {
            throw new Exceptions.UnsupportedImageExtensionException(fileExtension);
        }

        string imageUrl = $"https://placeholder-firebase-url.com/{req.ImageFile.FileName}";

        // Update product image
        product.ImageUrl = imageUrl;
        product.UpdatedAtUtc = DateTime.UtcNow;

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(ct);

        return new Response(
            product.Id,
            product.ImageUrl,
            product.UpdatedAtUtc.Value);
    }
}
