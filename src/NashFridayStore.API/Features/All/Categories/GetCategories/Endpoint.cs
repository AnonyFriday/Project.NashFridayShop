using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.All.Categories.GetCategories;

[AllowAnonymous]
[ApiController]
[Route("api/all/categories")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet]
    [Tags("Public - Categories")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] Request request,
        CancellationToken ct
    )
    {
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
