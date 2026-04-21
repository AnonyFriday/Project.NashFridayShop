using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Commons;
using NashFridayStore.Domain.Entities;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace NashFridayStore.API.Features.Products.PostProductRating;

#region Contracts
public record Request(Guid ProductId, RequestBody RequestBody);
public record RequestBody(string? Comment, int Stars = AppCts.Api.MinStars);
public record Response(Guid ProductId, int Stars, string? Comment, DateTime CreatedAtUtc);
#endregion

#region Validation
public sealed class Validator : AbstractValidator<Request>
{
    public const string ProductIdIsRequired = "Product Id is required.";
    public const string StarsInRange = "Stars must be between 1 and 10.";
    public const string CommentNotExceedLength = "Comment must not exceed 1000 characters.";

    public Validator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(ProductIdIsRequired);

        RuleFor(x => x.RequestBody.Stars)
            .InclusiveBetween(AppCts.Api.MinStars, AppCts.Api.MaxStars)
            .WithMessage(StarsInRange)
            .OverridePropertyName("Stars");

        RuleFor(x => x.RequestBody.Comment)
            .MaximumLength(1000)
            .WithMessage(CommentNotExceedLength)
            .OverridePropertyName("Comment");
    }
}
#endregion

#region Business Logic
public sealed class Handler(StoreDbContext dbContext, IValidator<Request> validator)
{
    public async Task<Response> HandleAsync(Request orgReq, CancellationToken ct)
    {
        // Cleaning Request
        Request req = orgReq with
        {
            RequestBody = orgReq.RequestBody with
            {
                Comment = string.IsNullOrWhiteSpace(orgReq.RequestBody.Comment)
                    ? null
                    : orgReq.RequestBody.Comment.Trim(),
            }
        };

        // Handle Validation
        ValidationResult validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
        {
            throw PostProductRatingErrors.Validation(validation.Errors);
        }

        // Business Logic
        Product? product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.Id == req.ProductId, cancellationToken: ct);

        if (product == null)
        {
            throw PostProductRatingErrors.ProductNotFound(req.ProductId);
        }

        var newRating = new ProductRating
        {
            Id = Guid.NewGuid(),
            ProductId = req.ProductId,
            Stars = req.RequestBody.Stars,
            Comment = req.RequestBody.Comment,
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.ProductRatings.Add(newRating);
        await dbContext.SaveChangesAsync(ct);

        return new Response(
            newRating.ProductId,
            newRating.Stars,
            newRating.Comment,
            newRating.CreatedAtUtc);
    }
}
#endregion

#region Endpoints
[ApiController]
[Route("api/products/{productId:guid}/rating")]
public class PostProductRatingController(Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromRoute] Guid ProductId,
        [FromBody] RequestBody body,
        CancellationToken ct
    )
    {
        var request = new Request(ProductId, body);
        Response response = await handler.HandleAsync(request, ct);
        return CreatedAtAction(nameof(Post), response);
    }
}
#endregion
