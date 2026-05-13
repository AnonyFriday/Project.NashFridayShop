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

        builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(100);

        builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(150);

        builder.Property(x => x.AvatarUrl)
                .IsRequired(false)
                .HasMaxLength(500);

        builder.Property(x => x.Address)
                .IsRequired(false)
                .HasMaxLength(300);

        builder.ConfigureSoftDeletable("users");
        builder.ConfigureAuditable();
    }
}
