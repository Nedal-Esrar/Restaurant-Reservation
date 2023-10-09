namespace RestaurantReservation.Db.Models;

public class Table
{
  public int TableId { get; set; }

  public int RestaurantId { get; set; }

  public Restaurant Restaurant { get; set; }

  public int Capacity { get; set; }

  public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}