using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Products.GetProduct;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/products/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        [FromQuery] bool IncludeDeleted, CancellationToken ct)
    {
        var request = new Request(id, IncludeDeleted);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
