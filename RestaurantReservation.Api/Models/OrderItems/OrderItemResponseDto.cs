namespace RestaurantReservation.Api.Models.OrderItems;

public class OrderItemResponseDto
{
  public int OrderItemId { get; set; }
  
  public int OrderId { get; set; }
  
  public int MenuItemId { get; set; }
  
  public int Quantity { get; set; }
}