using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Products.UpdateProduct;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/products/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut]
    [Tags("Admin - Products")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Put(
        [FromRoute] Guid id,
        [FromQuery] bool includeDeleted,
        [FromBody] RequestBody body,
        CancellationToken ct
    )
    {
        var request = new Request(
            id,
            body,
            includeDeleted);

        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
