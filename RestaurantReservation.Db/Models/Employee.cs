using RestaurantReservation.Db.Models.Enums;

namespace RestaurantReservation.Db.Models;

public class Employee : Entity
{
  public int RestaurantId { get; set; }

  public Restaurant Restaurant { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public ICollection<Order> Orders { get; set; } = new List<Order>();

  public EmployeePosition Position { get; set; }
}