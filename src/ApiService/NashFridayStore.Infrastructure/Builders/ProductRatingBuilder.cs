using NashFridayStore.Domain.Entities;

namespace NashFridayStore.Infrastructure.Builders;

public sealed class ProductRatingBuilder
{
    private Guid _productId;
    private int _stars = 5;
    private string? _comment;
    private DateTime _createdAtUtc = DateTime.UtcNow;
    private DateTime? _updatedAtUtc;
    private bool _isDeleted;
    private DateTime? _deletedAtUtc;

    public ProductRatingBuilder WithProductId(Guid productId)
    {
        _productId = productId;
        return this;
    }

    public ProductRatingBuilder WithStars(int stars)
    {
        _stars = stars;
        return this;
    }

    public ProductRatingBuilder WithComment(string? comment)
    {
        _comment = string.IsNullOrWhiteSpace(comment)
            ? null
            : comment.Trim();
        return this;
    }

    public ProductRatingBuilder WithCreatedAtUtc(DateTime createdAtUtc)
    {
        _createdAtUtc = createdAtUtc;
        return this;
    }

    public ProductRatingBuilder WithUpdatedAtUtc(DateTime updatedAtUtc)
    {
        _updatedAtUtc = updatedAtUtc;
        return this;
    }

    public ProductRatingBuilder WithIsDeleted(bool isDeleted)
    {
        _isDeleted = isDeleted;
        _deletedAtUtc = DateTime.UtcNow;
        return this;
    }

    public ProductRating Build()
    {
        return new ProductRating
        {
            Id = Guid.NewGuid(),
            ProductId = _productId,
            Stars = _stars,
            Comment = _comment,
            CreatedAtUtc = _createdAtUtc,
            UpdatedAtUtc = _updatedAtUtc,
            IsDeleted = _isDeleted,
            DeletedAtUtc = _deletedAtUtc
        };
    }
}
