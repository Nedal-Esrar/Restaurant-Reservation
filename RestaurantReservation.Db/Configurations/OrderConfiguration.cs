using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.Property(o => o.Id).HasColumnName("order_id");
    builder.Property(o => o.EmployeeId).HasColumnName("employee_id");
    builder.Property(o => o.OrderDate).HasColumnName("order_date");
    builder.Property(o => o.ReservationId).HasColumnName("reservation_id");
    builder.Property(o => o.TotalAmount).HasColumnType("decimal(8, 2)").HasColumnName("total_amount");

    builder.HasOne(o => o.Employee)
      .WithMany(e => e.Orders)
      .HasForeignKey(o => o.EmployeeId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasOne(o => o.Reservation)
      .WithMany(r => r.Orders)
      .HasForeignKey(o => o.ReservationId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.NoAction);
  }
}