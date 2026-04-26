using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Categories.GetCategories;

[ApiController]
[Route("api/categories")]
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
