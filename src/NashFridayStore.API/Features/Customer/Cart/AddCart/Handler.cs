using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NashFridayStore.Domain.Entities.Carts;
using NashFridayStore.Domain.Entities.Products;
using NashFridayStore.Infrastructure.Data;
using NashFridayStore.Infrastructure.Interfaces;

namespace NashFridayStore.API.Features.Customer.Cart.AddCart;

public sealed class Handler()
{
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

    public async Task<Response> HandleAsync(Request req, CancellationToken ct)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static

    {
        return new Response();
    }
}
