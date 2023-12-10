using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    builder.Property(c => c.Id).HasColumnName("customer_id");
    builder.Property(c => c.Email).HasColumnName("email");
    builder.Property(c => c.FirstName).HasColumnName("first_name");
    builder.Property(c => c.LastName).HasColumnName("last_name");
    builder.Property(c => c.PhoneNumber).HasColumnName("phone_number");
  }
}