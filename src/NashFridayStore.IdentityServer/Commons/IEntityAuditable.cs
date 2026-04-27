namespace NashFridayStore.IdentityServer.Commons;

public interface IEntityAuditable
{
    DateTime CreatedAtUtc { get; set; }
    DateTime? UpdatedAtUtc { get; set; }
}
