namespace NashFridayStore.Domain.Entities;

internal class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; }
    public ICollection<Product> Products { get; set; }
}
