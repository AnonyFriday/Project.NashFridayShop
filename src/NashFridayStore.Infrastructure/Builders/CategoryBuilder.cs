using NashFridayStore.Domain.Entities;

namespace NashFridayStore.Infrastructure.Builders;

public class CategoryBuilder
{
    private string _name = "Test Category";
    private string _description = "Test Description";

    public CategoryBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CategoryBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public Category Build()
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = _name,
            Description = _description
        };
    }
}
