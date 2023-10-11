using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
  public void Configure(EntityTypeBuilder<Restaurant> builder)
  {
    builder.Property(r => r.Id).HasColumnName("restaurant_id");
    builder.Property(r => r.Address).HasColumnName("address");
    builder.Property(r => r.Name).HasColumnName("name");
    builder.Property(r => r.OpeningHours).HasColumnName("opening_hours");
    builder.Property(r => r.PhoneNumber).HasColumnName("phone_number");
  }
}