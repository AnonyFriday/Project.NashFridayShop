namespace NashFridayStore.Domain.Commons;

public interface IEntitySoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}
