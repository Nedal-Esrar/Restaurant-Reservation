using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
  public void Configure(EntityTypeBuilder<Reservation> builder)
  {
    builder.Property(r => r.Id).HasColumnName("reservation_id");
    builder.Property(r => r.CustomerId).HasColumnName("customer_id");
    builder.Property(r => r.PartySize).HasColumnName("party_size");
    builder.Property(r => r.ReservationDate).HasColumnName("reservation_date");
    builder.Property(r => r.RestaurantId).HasColumnName("restaurant_id");
    builder.Property(r => r.TableId).HasColumnName("table_id");

    builder.HasOne(r => r.Customer)
      .WithMany(c => c.Reservations)
      .HasForeignKey(r => r.CustomerId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(r => r.Restaurant)
      .WithMany(rr => rr.Reservations)
      .HasForeignKey(r => r.RestaurantId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(r => r.Table)
      .WithMany(t => t.Reservations)
      .HasForeignKey(r => r.TableId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.NoAction);
  }
}