using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Admin.Categories.CreateCategory;

[Authorize(Roles = AppCts.Identity.Roles.Admin)]
[ApiController]
[Route("api/admin/categories")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Create a new category
    /// </summary>
    [HttpPost]
    [Tags("Admin - Categories")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(
        [FromBody] Request request,
        CancellationToken ct)
    {
        Response response = await handler.HandleAsync(request, ct);
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }
}
