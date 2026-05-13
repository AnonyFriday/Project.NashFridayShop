using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.All.Categories.GetCategory;

[AllowAnonymous]
[ApiController]
[Route("api/all/categories/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Get a category by ID
    /// </summary>
    [HttpGet]
    [Tags("Public - Categories")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
