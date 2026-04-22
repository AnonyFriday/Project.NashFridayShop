using NashFridayStore.Domain.Commons;

namespace NashFridayStore.Domain.Entities;

public class ProductRating : IEntityAuditable, IEntitySoftDeletable
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}
