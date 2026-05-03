using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Categories.UpdateCategory;

[ApiController]
[Route("api/categories")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] RequestBody body,
        CancellationToken ct)
    {
        Response response = await handler.HandleAsync(new Request(id, body), ct);
        return Ok(response);
    }
}
