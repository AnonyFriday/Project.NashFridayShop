using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.API.Features.Admin.Categories.GetCategory;

[ApiController]
[Route("api/admin/categories/{id:guid}")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new Request(id);
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
