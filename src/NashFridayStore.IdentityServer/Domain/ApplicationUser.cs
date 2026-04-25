using Microsoft.AspNetCore.Identity;
using NashFridayStore.IdentityServer.Commons;

namespace NashFridayStore.IdentityServer.Domain;

public class ApplicationUser : IdentityUser<Guid>, IEntityAuditable, IEntitySoftDeletable
{
    public string FullName { get; set; }
    public string? Address { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
