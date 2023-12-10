using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Extensions;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Entities;

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
  
  public DbSet<EmployeeWithDetails> EmployeesWithDetails { get; set; }

  public DbSet<Table> Tables { get; set; }
  
  public DbSet<User> Users { get; set; }
  
  public DbSet<Role> Roles { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    
    modelBuilder.Seed();

    modelBuilder.Entity<ReservationWithDetails>().HasNoKey().ToView(nameof(ReservationsWithDetails));
    
    modelBuilder.Entity<EmployeeWithDetails>().HasNoKey().ToView(nameof(EmployeesWithDetails));
    
    modelBuilder.HasDbFunction(
      typeof(RestaurantReservationDbContext).GetMethod(
        nameof(CalculateRestaurantRevenue),
        new[] { typeof(int) }
      )!
    ).HasName("fn_CalculateRestaurantRevenue");
  }

  public decimal CalculateRestaurantRevenue(int restaurantId) => throw new NotSupportedException();
}