using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Infrastructure.Data;

namespace NashFridayStore.API.Features.Categories.CreateCategory;

public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Clean request
        Request req = orgReq with
        {
            Name = orgReq.Name.Trim(),
            Description = orgReq.Description.Trim()
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw new Exceptions.ValidationException(validation.Errors);
        }

        // Check if category name already exists
        bool nameExists = await dbContext.Categories
            .AsNoTracking()
            .AnyAsync(x => x.Name == req.Name, ct);

        if (nameExists)
        {
            throw new Exceptions.CategoryAlreadyExistsException(req.Name);
        }

        // Create category
        Category category = new()
        {
            Id = Guid.NewGuid(),
            Name = req.Name,
            Description = req.Description
        };

        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync(ct);

        return new Response(
            category.Id,
            category.Name,
            category.Description);
    }
}
