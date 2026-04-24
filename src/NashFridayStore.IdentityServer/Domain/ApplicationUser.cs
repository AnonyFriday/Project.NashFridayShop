using Microsoft.AspNetCore.Identity;
using NashFridayStore.IdentityServer.Commons;

namespace NashFridayStore.IdentityServer.Domain;

public class ApplicationUser : IdentityUser<Guid>, IEntityAuditable, IEntitySoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
