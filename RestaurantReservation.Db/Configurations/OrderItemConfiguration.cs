using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
  public void Configure(EntityTypeBuilder<OrderItem> builder)
  {
    builder.Property(i => i.OrderItemId).HasColumnName("order_item_id");
    builder.Property(i => i.ItemId).HasColumnName("item_id");
    builder.Property(i => i.OrderId).HasColumnName("order_id");
    builder.Property(i => i.Quantity).HasColumnName("quantity");

    builder.HasOne(i => i.Item)
      .WithMany(mi => mi.OrderItems)
      .HasForeignKey(i => i.ItemId)
      .OnDelete(DeleteBehavior.ClientSetNull);

    builder.HasOne(i => i.Order)
      .WithMany(o => o.OrderItems)
      .HasForeignKey(i => i.OrderId)
      .OnDelete(DeleteBehavior.ClientSetNull);
  }
}