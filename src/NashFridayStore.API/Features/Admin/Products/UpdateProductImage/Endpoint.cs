using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Products.UpdateProductImage;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/products/{productId:guid}/image")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Update product image
    /// </summary>
    [HttpPatch]
    [Tags("Admin - Products")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
