namespace RestaurantReservation.Db.Models;

public class Order
{
  public int OrderId { get; set; }

  public int ReservationId { get; set; }

  public Reservation Reservation { get; set; }

  public int EmployeeId { get; set; }

  public Employee Employee { get; set; }

  public DateTime OrderDate { get; set; }

  public int TotalAmount { get; set; }

  public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}