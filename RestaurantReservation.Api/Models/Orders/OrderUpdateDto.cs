namespace RestaurantReservation.Api.Models.Orders;

public class OrderUpdateDto
{
  /// <summary>
  /// The ID of the new reservation for the order.
  /// </summary>
  public int ReservationId { get; set; }

  /// <summary>
  /// The ID of the new employee for the order.
  /// </summary>
  public int EmployeeId { get; set; }

  /// <summary>
  /// The updated total amount of the order.
  /// </summary>
  public decimal TotalAmount { get; set; }
}