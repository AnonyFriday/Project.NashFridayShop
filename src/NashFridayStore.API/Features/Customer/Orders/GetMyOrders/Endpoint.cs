using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Customer.Orders.GetMyOrders;

[Authorize(Roles = AppCts.Identity.Roles.Customer)]
[ApiController]
[Route("api/customer/orders")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    /// <summary>
    /// Get the current customer's order history
    /// </summary>
    [HttpGet]
    [Tags("Customer - Orders")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Response>> GetMyOrders(CancellationToken ct)
    {
        Response response = await handler.HandleAsync(ct);
        return Ok(response);
    }
}
