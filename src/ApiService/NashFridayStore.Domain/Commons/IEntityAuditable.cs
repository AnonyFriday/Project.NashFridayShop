namespace NashFridayStore.Domain.Commons;

public interface IEntityAuditable
{
    DateTime CreatedAtUtc { get; set; }
    DateTime? UpdatedAtUtc { get; set; }
}
