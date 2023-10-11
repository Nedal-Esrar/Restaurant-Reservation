using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
  public void Configure(EntityTypeBuilder<Employee> builder)
  {
    builder.Property(e => e.Id).HasColumnName("employee_id");
    builder.Property(e => e.FirstName).HasColumnName("first_name");
    builder.Property(e => e.LastName).HasColumnName("last_name");
    builder.Property(e => e.Position).HasColumnName("position");
    builder.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

    builder.HasOne(e => e.Restaurant)
      .WithMany(r => r.Employees)
      .HasForeignKey(e => e.RestaurantId)
      .OnDelete(DeleteBehavior.ClientSetNull);
  }
}