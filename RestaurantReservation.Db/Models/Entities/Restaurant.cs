namespace RestaurantReservation.Db.Models.Entities;

public class Restaurant : Entity
{
  public string Name { get; set; }

  public string Address { get; set; }

  public string PhoneNumber { get; set; }

  public string OpeningHours { get; set; }

  public ICollection<Employee> Employees { get; set; } = new List<Employee>();

  public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

  public ICollection<Table> Tables { get; set; } = new List<Table>();

  public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}