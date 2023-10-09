using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
  public void Configure(EntityTypeBuilder<Reservation> builder)
  {
    builder.Property(r => r.ReservationId).HasColumnName("reservation_id");
    builder.Property(r => r.CustomerId).HasColumnName("customer_id");
    builder.Property(r => r.PartySize).HasColumnName("party_size");
    builder.Property(r => r.ReservationDate).HasColumnName("reservation_date");
    builder.Property(r => r.RestaurantId).HasColumnName("restaurant_id");
    builder.Property(r => r.TableId).HasColumnName("table_id");

    builder.HasOne(r => r.Customer)
      .WithMany(c => c.Reservations)
      .HasForeignKey(r => r.CustomerId)
      .OnDelete(DeleteBehavior.ClientSetNull);

    builder.HasOne(r => r.Restaurant)
      .WithMany(rr => rr.Reservations)
      .HasForeignKey(r => r.RestaurantId)
      .OnDelete(DeleteBehavior.ClientSetNull);

    builder.HasOne(r => r.Table)
      .WithMany(t => t.Reservations)
      .HasForeignKey(r => r.TableId)
      .OnDelete(DeleteBehavior.ClientSetNull);
  }
}