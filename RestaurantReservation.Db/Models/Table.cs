namespace RestaurantReservation.Db.Models;

public class Table : Entity
{
  public int RestaurantId { get; set; }

  public Restaurant Restaurant { get; set; }

  public int Capacity { get; set; }

  public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}