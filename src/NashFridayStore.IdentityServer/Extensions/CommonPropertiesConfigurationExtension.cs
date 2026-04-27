using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NashFridayStore.IdentityServer.Commons;

namespace NashFridayStore.IdentityServer.Extensions;

public static class CommonPropertiesConfigurationExtension
{
    public static void ConfigureAuditable<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityAuditable
    {
        builder.Property(x => x.CreatedAtUtc)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .ValueGeneratedOnAddOrUpdate()
            .IsRequired(false);
    }

    public static void ConfigureSoftDeletable<T>(this EntityTypeBuilder<T> builder, string tableName)
       where T : class, IEntitySoftDeletable
    {
        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.DeletedAtUtc)
            .IsRequired(false);

        builder.HasIndex(x => x.IsDeleted)
            .HasDatabaseName($"ix_{tableName}_is_deleted");
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
