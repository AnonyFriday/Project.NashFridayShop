using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Products.PostProductRating;

public record Request(Guid ProductId, RequestBody RequestBody);

public record RequestBody(string? Comment, int Stars = AppCts.Api.MinStars);
