using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Products.ToggleDeleteProduct;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/products/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPatch]
    public async Task<IActionResult> ToggleDelete(
        [FromRoute] Guid id,
        CancellationToken ct
    )
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
