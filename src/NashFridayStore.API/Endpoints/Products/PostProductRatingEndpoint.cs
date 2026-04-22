using Microsoft.AspNetCore.Mvc;
using NashFridayStore.SharedFeatures.Features.Products.PostProductRating;

namespace NashFridayStore.API.Endpoints.Products;

[ApiController]
[Route("api/products/{productId:guid}/rating")]
public sealed class PostProductRatingEndpoint(Handler handler) : ControllerBase
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
