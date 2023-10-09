using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
  public void Configure(EntityTypeBuilder<MenuItem> builder)
  {
    builder.HasKey(mi => mi.ItemId);

    builder.Property(mi => mi.ItemId).HasColumnName("item_id");
    builder.Property(mi => mi.Description).HasColumnName("description");
    builder.Property(mi => mi.Name).HasColumnName("name");
    builder.Property(mi => mi.Price).HasColumnType("decimal(8, 2)").HasColumnName("price");
    builder.Property(mi => mi.RestaurantId).HasColumnName("restaurant_id");

    builder.HasOne(mi => mi.Restaurant)
      .WithMany(r => r.MenuItems)
      .HasForeignKey(mi => mi.RestaurantId)
      .OnDelete(DeleteBehavior.ClientSetNull);
  }
}