using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Products.GetProducts;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/products")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Request request,
        CancellationToken ct
    )
    {
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
