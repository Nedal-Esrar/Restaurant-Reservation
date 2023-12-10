namespace RestaurantReservation.Db.Models.Entities;

public class Reservation : Entity
{
  public int? CustomerId { get; set; }

  public Customer Customer { get; set; }

  public int? RestaurantId { get; set; }

  public Restaurant Restaurant { get; set; }

  public int? TableId { get; set; }

  public Table Table { get; set; }

  public DateTime ReservationDate { get; set; }

  public int PartySize { get; set; }

  public ICollection<Order> Orders { get; set; } = new List<Order>();
}