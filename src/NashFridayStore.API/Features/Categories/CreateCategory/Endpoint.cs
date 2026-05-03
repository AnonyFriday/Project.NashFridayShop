using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Categories.CreateCategory;

[ApiController]
[Route("api/categories")]
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
