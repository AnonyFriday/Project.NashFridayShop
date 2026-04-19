using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;


namespace NashFridayStore.API.Features.Products.GetProduct;

#region Contracts
public sealed record Request(Guid Id);

public sealed record Response(
    Guid Id,
    string Name,
    string ImageUrl,
    decimal PriceUsd,
    ProductStatus Status,
    decimal AverageStars);
#endregion

#region Validation
public sealed class Validator : AbstractValidator<Request>
{
    public const string IdRequired = "Product Id is required.";

    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(IdRequired);
    }
}
#endregion

#region Business Logic
public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> Handle(Request req, CancellationToken ct)
    {
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw GetProductErrors.Validation(validation.Errors);
        }

        Response? product = await dbContext.Products
            .AsNoTracking()
            .Include(p => p.ProductRatings)
            .Where(x => x.Id == req.Id)
            .Select(x => new Response(
                x.Id,
                x.Name,
                x.ImageUrl,
                x.PriceUsd,
                x.Status,
                x.ProductRatings.Any() ? x.ProductRatings.Average(r => (decimal)r.Stars) % AppCts.Api.MaxStars : 0))
            .FirstOrDefaultAsync(ct);

        if (product is null)
        {
            throw GetProductErrors.NotFound(req.Id);
        }

        return product;
    }
}
#endregion

#region Endpoints
[ApiController]
[Route("api/products/{id:guid}")]
public class GetProductController(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.Handle(request, ct);
        return Ok(response);
    }
}
#endregion
