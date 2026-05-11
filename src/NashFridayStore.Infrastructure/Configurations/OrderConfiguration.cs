using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NashFridayStore.Domain.Entities.Orders;
using NashFridayStore.Infrastructure.Extensions;

namespace NashFridayStore.Infrastructure.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.CustomerFullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CustomerEmail)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.DeliveryAddress)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.TotalPriceInUsd)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.StripeCheckoutSessionId)
            .HasMaxLength(300);

        builder.Property(x => x.StripePaymentIntentId)
            .HasMaxLength(300);

        builder.Property(x => x.OrderStatus)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<OrderStatus>>();

        builder.Property(x => x.PaymentStatus)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<PaymentStatus>>();

        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ConfigureAuditable();
    }
}
