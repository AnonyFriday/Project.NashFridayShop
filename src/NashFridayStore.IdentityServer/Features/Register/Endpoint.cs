using Microsoft.AspNetCore.Mvc;

namespace NashFridayStore.IdentityServer.Features.Register;

[ApiController]
[Route("api/auth/register")]
public class Endpoint(Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterAsync([FromBody] Request request, CancellationToken ct)
    {
        Response response = await handler.HandleAsync(request, ct);
        return Ok(response);
    }
}
