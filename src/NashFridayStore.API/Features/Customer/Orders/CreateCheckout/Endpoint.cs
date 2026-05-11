using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NashFridayStore.Domain.Commons;

namespace NashFridayStore.API.Features.Customer.Orders.CreateCheckout;

[Authorize(Roles = AppCts.Identity.Roles.Customer)]
[ApiController]
[Route("customer/orders/checkout")]
public sealed class Endpoint(Handler handler) : ControllerBase
{
    [HttpPost]
    public async Task<Response> Post(
        [FromBody] Request request,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(request, cancellationToken);
    }
}