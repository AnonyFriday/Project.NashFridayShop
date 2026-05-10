using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Products.UpdateProduct;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/products/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPut]
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
