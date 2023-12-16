using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasIndex(u => u.Username).IsUnique();
    
    builder.HasMany(u => u.Roles)
      .WithMany(r => r.Users);
  }
}