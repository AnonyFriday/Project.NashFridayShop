using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NashFridayStore.Domain.Entities.Products;

namespace NashFridayStore.Infrastructure.Builders;

public sealed class ProductBuilder
{
    private Guid _categoryId;
    private string _name = "Test Product";
    private string _description = "Test Description";
    private string _imageUrl = "https://picsum.photos/300";
    private decimal _price = 10;
    private int _quantity = 10;
    private DateTime _createdAtUtc = DateTime.UtcNow;
    private DateTime? _updatedAtUtc;

    private ProductStatus _status = ProductStatus.InStock;

    public ProductBuilder WithCategoryId(Guid categoryId)
    {
        _categoryId = categoryId;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithImageUrl(string imageUrl)
    {
        _imageUrl = imageUrl;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public ProductBuilder WithStatus(ProductStatus status)
    {
        _status = status;
        return this;
    }

    public ProductBuilder WithUpdatedAtUtc(DateTime updatedAtUtc)
    {
        _updatedAtUtc = updatedAtUtc;
        return this;
    }

    public ProductBuilder WithCreatedAtUtc(DateTime createdAtUtc)
    {
        _createdAtUtc = createdAtUtc;
        return this;
    }

    public Product Build()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            CategoryId = _categoryId,
            Name = _name,
            Description = _description,
            ImageUrl = _imageUrl,
            PriceUsd = _price,
            Quantity = _quantity,
            Status = _status,
            CreatedAtUtc = _createdAtUtc,
            UpdatedAtUtc = _updatedAtUtc
        };
    }
}
