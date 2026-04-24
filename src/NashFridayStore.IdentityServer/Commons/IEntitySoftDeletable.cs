namespace NashFridayStore.IdentityServer.Commons;

public interface IEntitySoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}
