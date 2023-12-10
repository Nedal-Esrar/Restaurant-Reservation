namespace RestaurantReservation.Api.Models.Orders;

public class OrderCreationDto
{
  /// <summary>
  /// The ID of the reservation for the new order.
  /// </summary>
  public int ReservationId { get; set; }

  /// <summary>
  /// The ID of the employee how took the order.
  /// </summary>
  public int EmployeeId { get; set; }

  /// <summary>
  /// The date when the order is taken.
  /// </summary>
  public DateTime OrderDate { get; set; }

  /// <summary>
  /// The total amount of the order.
  /// </summary>
  public decimal TotalAmount { get; set; }
}