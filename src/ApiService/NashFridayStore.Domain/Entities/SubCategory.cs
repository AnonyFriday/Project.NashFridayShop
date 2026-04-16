using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.Domain.Entities;

public sealed class SubCategory
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ICollection<Product> Products { get; set; }
}
