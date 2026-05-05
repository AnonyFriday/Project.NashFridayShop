using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Admin.Products.UpdateProductImage;

[ApiController]
[Route("api/admin/products/{productId:guid}/image")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPatch]
    public async Task<IActionResult> Patch(
        [FromRoute] Guid productId,
        [FromQuery] bool includeDeleted,
        [FromForm] IFormFile imageFile,
        CancellationToken ct
    )
    {
        var request = new Request(productId, imageFile, includeDeleted);
        Response response = await handler.HandleAsync(request, ct);

        return Ok(response);
    }
}
