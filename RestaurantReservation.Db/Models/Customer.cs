namespace RestaurantReservation.Db.Models;

public class Customer : Entity
{
  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string Email { get; set; }

  public string PhoneNumber { get; set; }

  public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}