using System;
using System.Collections.Generic;
using System.Text;

namespace NashFridayStore.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid SubcategoryId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal PriceUsd { get; set; }
    public string ImageUrl { get; set; } = default!;
    public int Quantity { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
