using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db;

public class RestaurantReservationDbContext : DbContext
{
  public RestaurantReservationDbContext(DbContextOptions<RestaurantReservationDbContext> options) : base(options)
  {
  }
  
  public DbSet<Customer> Customers { get; set; }

  public DbSet<Employee> Employees { get; set; }

  public DbSet<MenuItem> MenuItems { get; set; }

  public DbSet<Order> Orders { get; set; }

  public DbSet<OrderItem> OrderItems { get; set; }

  public DbSet<Reservation> Reservations { get; set; }

  public DbSet<Restaurant> Restaurants { get; set; }
  
  public DbSet<ReservationWithDetails> ReservationsWithDetails { get; set; }

  public DbSet<Table> Tables { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    
    DataSeeding.Seed(modelBuilder);

    modelBuilder.Entity<ReservationWithDetails>().HasNoKey().ToView(nameof(ReservationsWithDetails));
  }
}