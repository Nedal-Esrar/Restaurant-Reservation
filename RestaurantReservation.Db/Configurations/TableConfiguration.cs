using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class TableConfiguration : IEntityTypeConfiguration<Table>
{
  public void Configure(EntityTypeBuilder<Table> builder)
  {
    builder.Property(t => t.Id).HasColumnName("table_id");
    builder.Property(t => t.Capacity).HasColumnName("capacity");
    builder.Property(t => t.RestaurantId).HasColumnName("restaurant_id");

    builder.HasOne(t => t.Restaurant)
      .WithMany(r => r.Tables)
      .HasForeignKey(t => t.RestaurantId)
      .OnDelete(DeleteBehavior.ClientSetNull);
  }
}