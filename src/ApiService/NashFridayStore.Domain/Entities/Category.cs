using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; }
    public ICollection<Product> Products { get; set; }
}
