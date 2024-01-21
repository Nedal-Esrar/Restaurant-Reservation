namespace RestaurantReservation.Api.Models.Orders;

public class OrderResponseDto
{
  public int Id { get; set; }
  
  public int? ReservationId { get; set; }

  public int? EmployeeId { get; set; }

  public DateTime OrderDate { get; set; }

  public decimal TotalAmount { get; set; }
}