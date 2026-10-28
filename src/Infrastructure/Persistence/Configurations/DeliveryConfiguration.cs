using LastMile.Domain.Entregas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastMile.Infrastructure.Persistence.Configurations;

public sealed class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("deliveries");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => new DeliveryId(value))
            .HasColumnName("id");

        builder.Property(d => d.OrderId)
            .HasConversion(
                id => id.Value,
                value => new OrderId(value))
            .HasColumnName("order_id");

        builder.Property(d => d.CourierId)
            .HasConversion(
                id => id!.Value,
                value => new CourierId(value))
            .HasColumnName("courier_id");

        builder.Property(d => d.Status)
            .HasConversion<string>()
            .HasColumnName("status");

        builder.Property(d => d.AssignedAt)
            .HasColumnName("assigned_at");

        builder.OwnsOne(d => d.Address, a =>
        {
            a.Property(x => x.Street).HasColumnName("address_street");
            a.Property(x => x.City).HasColumnName("address_city");
            a.Property(x => x.Latitude).HasColumnName("address_lat");
            a.Property(x => x.Longitude).HasColumnName("address_lng");
            a.Property(x => x.Reference).HasColumnName("address_ref");
        });

        builder.OwnsOne(d => d.Window, w =>
        {
            w.Property(x => x.Start).HasColumnName("window_start");
            w.Property(x => x.End).HasColumnName("window_end");
        });

        builder.Ignore(d => d.DomainEvents);
    }
}