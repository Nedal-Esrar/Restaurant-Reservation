namespace RestaurantReservation.Db.Models.Entities;

public class Order : Entity
{
  public int? ReservationId { get; set; }

  public Reservation Reservation { get; set; }

  public int? EmployeeId { get; set; }

  public Employee Employee { get; set; }

  public DateTime OrderDate { get; set; }

  public decimal TotalAmount { get; set; }

  public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}