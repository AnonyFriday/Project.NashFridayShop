using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NashFridayStore.IdentityServer.Domain;
using NashFridayStore.IdentityServer.Extensions;

namespace NashFridayStore.IdentityServer.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.ConfigureSoftDeletable("users");
        builder.ConfigureAuditable();
    }
}
