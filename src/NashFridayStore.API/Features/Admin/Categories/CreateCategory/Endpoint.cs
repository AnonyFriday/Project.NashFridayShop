using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Categories.CreateCategory;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/categories")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] Request request,
        CancellationToken ct)
    {
        Response response = await handler.HandleAsync(request, ct);
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }
}
